using NoSQL.Models;
using NoSQL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NoSQL.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        MongoAlbumPerformer mongod = new MongoAlbumPerformer("test","albums");

        [HttpPost]
        public ActionResult AlbumPreview(Photo model, HttpPostedFileBase file, string albumName, string delete, string phot)
        {
            if (delete == "false")
            {
                if (file != null)
                {
                    if (!file.ContentType.StartsWith("image"))
                    {
                        ModelState.AddModelError("file", "Należy wybrać prawidłowy format zdjęcia");
                    }
                    else
                    {
                        ServerPathFinder finder = new ServerPathFinder();
                        model.ServerPath = finder.GetBase64ImageString(file);
                    }

                    if (ModelState.IsValid)
                    {
                        mongod.UpdateAlbumAddPhoto(albumName, model);
                        ModelState.Clear();
                    }
                }
            }
            else
            {
                mongod.DeletePhotoFromAlbum(albumName, phot);
                foreach(var error in ModelState.Values)
                {
                    error.Errors.Clear();
                }
            }
            ViewBag.AlbumTitle = albumName;
            ViewBag.PhotoList = mongod.GetPicturesByAlbumName(albumName).Pictures;

            return View();
        }

        public ActionResult AlbumPreview(string Name)
        {
            var album = mongod.GetPicturesByAlbumName(Name);
            ViewBag.AlbumTitle = Name;
            ViewBag.PhotoList = album.Pictures;

            return View();
        }

        [HttpPost]
        public ActionResult Create(Album model, HttpPostedFileBase file)
        {
            if (!file.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("file", "Należy wybrać prawidłowy format zdjęcia");
            }
            else
            {
                ServerPathFinder finder = new ServerPathFinder();
                model.TitlePhoto.ServerPath = finder.GetBase64ImageString(file);               
            }

            if (ModelState.IsValid)
            {
                model.Owner = HttpContext.User.Identity.Name;
                mongod.CreateAlbum(model);
            }
            var albums = mongod.GetAlbumsByUserName(HttpContext.User.Identity.Name);
            ViewBag.Albums = albums;

            return View();
        }

        public ActionResult Create()
        {
            var albums = mongod.GetAlbumsByUserName(HttpContext.User.Identity.Name);
            ViewBag.Albums = albums;
            return View();
        }
    }
}
