using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.Helpers
{
    public static class FileManager
    {
        public static string Save(IFormFile formFile, string rootPath, string folders)
        {
            string newFileName = Guid.NewGuid().ToString() + (formFile.FileName.Length > 64 ? formFile.FileName.Substring(formFile.FileName.Length - 64) : formFile.FileName);
            string path = Path.Combine(rootPath,folders, newFileName);

            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return newFileName; 
        }

        public static void Delete(string rootPath, string folders, string fileName)
        {
            string path = Path.Combine(rootPath,folders, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
