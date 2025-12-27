using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ecomm.infrastructure.Services
{
    public class ImageManagementService : IImageManagementService
    {
        // مسار wwwroot سيتم بناءه حسب المشروع الذي يستخدم الـ Library
        private readonly string _webRootPath;

        public ImageManagementService()
        {
            // استخدام CurrentDirectory للوصول إلى المشروع الذي ينفذ الكود
            _webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var savedImages = new List<string>();

            if (files == null || files.Count == 0)
                return savedImages;

            var imageDirectory = Path.Combine(_webRootPath, "Images", src);

            if (!Directory.Exists(imageDirectory))
                Directory.CreateDirectory(imageDirectory);

            foreach (var file in files)
            {
                if (file == null) continue;

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(imageDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                savedImages.Add($"/Images/{src}/{fileName}");
            }

            return savedImages;
        }

        public void DeleteImageAsync(string src)
        {
            if (string.IsNullOrEmpty(src)) return;

            var relativePath = src.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Console.WriteLine($"Deleted image: {fullPath}");
            }
            else
            {
                Console.WriteLine($"Image not found: {fullPath}");
            }
        }

    }
}
