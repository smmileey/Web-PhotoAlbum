using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using NoSQL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NoSQL.Services
{
    public class MongoAlbumPerformer
    {
        protected static IMongoClient client;
        protected static IMongoDatabase database;
        private static IMongoCollection<Album> collection;
        private string collectionName;

        public MongoAlbumPerformer(string databaseName, string collectionName)
        {
            
            client = new MongoClient(ConfigurationManager.ConnectionStrings["mongoDB"].ConnectionString);
            database = client.GetDatabase(databaseName);
            this.collectionName = collectionName;
            collection = database.GetCollection<Album>(collectionName, new MongoCollectionSettings { AssignIdOnInsert = true });
        }

        public void SetCollection(string collectionName)
        {
            this.collectionName = collectionName;
            collection = database.GetCollection<Album>(collectionName);
        }

        public void CreateAlbum(Album album)
        {
            var document = new Album
            {
                Name = album.Name,
                Owner = HttpContext.Current.User.Identity.Name,
                Description = album.Description,
                CreationTime = DateTime.Now,
                TitlePhoto = album.TitlePhoto,
                Pictures = album.Pictures
            };

            collection.InsertOne(document);
        }

        public List<Album> GetAlbumsByUserName(string username)
        {
            var projection = Builders<Album>.Projection
                .Include(a => a.Name)
                .Include(a => a.Description)
                .Include(a => a.TitlePhoto)
                .Include(a=>a.CreationTime);

            var result = collection
                .Find(a => a.Owner == HttpContext.Current.User.Identity.Name)
                .Project<Album>(projection).ToList();

            return result;
        }

        public Album GetPicturesByAlbumName(string albumName)
        {
            var projection = Builders<Album>.Projection
                .Include(a => a.Pictures);

            var result = collection
                .Find(a => a.Owner == HttpContext.Current.User.Identity.Name & a.Name == albumName)
                .Project<Album>(projection).FirstOrDefault();

            return result;
        }

        public void UpdateAlbumAddPhoto(string albumName, Photo photo)
        {
            var builder = Builders<Album>.Filter;
            var filter = builder.Eq(f => f.Name, albumName) & builder.Eq(f => f.Owner, HttpContext.Current.User.Identity.Name);
            var result = collection.Find(filter).FirstOrDefault();

            if (result == null)
                throw new ArgumentException("No album of supplied name: {0}", albumName);
            else
            {
                        var picture = new Photo
                        {
                            PhotoName = photo.PhotoName,
                            PhotoDescription = photo.PhotoDescription,
                            ServerPath = photo.ServerPath,
                        };

                        var update = Builders<Album>.Update.Push(a => a.Pictures, picture);
                        collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert=true });
            }
        }

        public void DeletePhotoFromAlbum(string albumName, string photoName)
        {
            var builder = Builders<Album>.Filter;
            var filter = builder.Eq(f => f.Name, albumName) & builder.Eq(f => f.Owner, HttpContext.Current.User.Identity.Name);
            var result = collection.Find(filter).SingleOrDefault();

            if (result == null)
                throw new ArgumentException("No album of supplied name: {0}", albumName);
            else
            {
                var update = Builders<Album>.Update
                    .PullFilter(a => a.Pictures, Builders<Photo>.Filter.Eq(p => p.PhotoName, photoName));
                long count = collection.UpdateOne(filter, update).MatchedCount;
            }
        }
    }
}