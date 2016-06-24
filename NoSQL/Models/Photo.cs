using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace NoSQL.Models
{
    public class Photo : IEquatable<Photo>
    {
        [Required]
        public string PhotoName { get; set; }

        [Required]
        public string PhotoDescription { get; set; }

        public string ServerPath { get; set; }

        public Photo() { }

        public Photo(string name, string desc)
        {
            PhotoName = name;
            PhotoDescription = desc;
        }

        public bool Equals(Photo other)
        {
            return string.Equals(PhotoName, other.PhotoName);
        }
    }
}