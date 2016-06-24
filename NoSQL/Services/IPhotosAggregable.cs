using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoSQL.Services
{
    public interface IPhotosAggregable
    {
        IAlbumIterable GetIterator();
    }
}