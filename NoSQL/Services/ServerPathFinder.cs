using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NoSQL.Services
{
    public class ServerPathFinder
    {
        public string GetBase64ImageString(HttpPostedFileBase file)
        {
            string mime = Regex.Match(file.ContentType, @"(?<=image/)\w+").Value;
            byte[] bytes = new byte[file.ContentLength];
            file.InputStream.Read(bytes, 0, file.ContentLength);
            return string.Format("data:image/{0};base64,{1}",mime, Convert.ToBase64String(bytes));
        }
    }
}