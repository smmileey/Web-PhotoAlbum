using NoSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSQL.Services
{
    public interface IAlbumIterable
    {
        bool HasNext();
        Photo Current();
        Photo Next();
    }
}