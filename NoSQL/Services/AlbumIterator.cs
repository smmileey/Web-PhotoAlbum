using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoSQL.Models;

namespace NoSQL.Services
{
    public class AlbumIterator : IAlbumIterable
    {
        private Album collection;
        private int count;

        public AlbumIterator(Album album)
        {
            collection = album;
        }

        public Photo Current()
        {
            if (count < collection.Count)
                return collection[count++];
            else
                throw new IndexOutOfRangeException();
        }

        public bool HasNext()
        {
            if (count < collection.Count - 1)
                return true;
            else
                return false;
        }

        public Photo Next()
        {
            if (HasNext())
                return collection[++count];
            else
                throw new IndexOutOfRangeException();
        }
    }
}