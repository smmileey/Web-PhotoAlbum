using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using NoSQL.Services;

namespace NoSQL.Models
{
    public class Album : IPhotosAggregable
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Owner { get; set; }
        public Photo TitlePhoto { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local,Representation =BsonType.DateTime)]
        public DateTime CreationTime { get; set; }
        public IList<Photo> Pictures { get; set; }

        public Album() { Pictures = new List<Photo>(); TitlePhoto = new Photo(); }
        public Album(string name, string owner, Photo pic)
        {
            Name = name;
            Owner = owner;
            TitlePhoto = pic;
            Pictures = new List<Photo>();
            TitlePhoto = new Photo();
        }

        public bool InsertPicture(Photo pic)
        {
            if (!Pictures.Contains(pic))
            {
                Pictures.Add(pic);
                return true;
            }
            else
                throw new ArgumentException();
        }

        public bool InsertPictures(List<Photo> photos)
        {
            foreach(var photo in photos)
            {
                if (!Pictures.Contains(photo))
                {
                    Pictures.Add(photo);
                }
                else
                    throw new ArgumentException();
            }
            return true;
        }

        public bool RemovePicture(Photo pic)
        {
                Pictures.Remove(pic);
                return true;
        }

        public int Count
        {
            get { return Pictures.Count; }
        }

        public Photo this[int index]
        {
            get { return Pictures[index]; }
            set { Pictures[index] = value; }
        }

        public IAlbumIterable GetIterator()
        {
            return new AlbumIterator(this);
        }
    }
}