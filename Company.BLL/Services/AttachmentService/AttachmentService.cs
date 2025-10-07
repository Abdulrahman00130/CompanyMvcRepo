using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        List<string> availableExtensions = [".png", ".jpg", "jpeg"];
        int maxFileSize = 1024 * 1024 * 2;
        public string? Upload(IFormFile file, string folderName)
        {
            // 1. check extension
            var extension = Path.GetExtension(file.FileName);
            if (!availableExtensions.Contains(extension)) return null;

            // 2. check size
            if(file.Length == 0 || file.Length >  maxFileSize) return null;

            // 3. get located folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName);

            // 4. make attachment name unique (use GUID)
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            // 5. get file path
            var filePath = Path.Combine(folderPath, fileName);

            // 6. create file stream to copy file (unmanaged)
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            
            // 7. use stream to copy file
            file.CopyTo(fileStream);

            //8. return file name to store in database
            return fileName;
        }

        public bool Delete(string filePath)
        {
            if(!File.Exists(filePath)) return false;

            File.Delete(filePath);
            return true;
        }
    }
}
