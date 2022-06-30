using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Extensions
{
    public static class FileExtension
    {
        public static bool IsOkay(this IFormFile photo,int mb)
        {
            return photo.ContentType.Contains("image") && photo.Length > 1024 * 1024 * mb;
        }
    }
}
