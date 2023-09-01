using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.Helpers
{
    public class FileUploadHelper
    {
        public static async Task<FileInfo> UploadFile(IFormFile file, string fileName = null, bool useOriginalName = false, string path = null)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                if (useOriginalName)
                {
                    fileName = file.FileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString();
                }
            }

            if (!Path.HasExtension(fileName))
            {
                var extension = Path.GetExtension(file.FileName);
                fileName += extension;
            }

            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/Uploads/");
            }

            //Create path if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            //return path.Replace("wwwroot/", "");
            var fileData = new FileInfo(path);
            return fileData;
        }

        public static async Task<FileInfo> Base64Upload(string base64Filestring, string folderName, string fileName)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            byte[] imageBytes = Convert.FromBase64String(base64Filestring);
            var filePath = string.Format("{0}/{1}", dir, fileName);
            File.WriteAllBytes(filePath, imageBytes);

            var fileData = new FileInfo(filePath);

            return await Task.FromResult(fileData);
        }
        public static bool HasBinaryContent(string content)
        {
            return content.Any(ch => char.IsControl(ch) && ch != '\r' && ch != '\n');
        }
    }
}
