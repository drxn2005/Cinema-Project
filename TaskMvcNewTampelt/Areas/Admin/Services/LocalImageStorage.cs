using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace TaskMvcNewTampelt.Services
{
    public class LocalImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;
        public LocalImageStorage(IWebHostEnvironment env) => _env = env;

        public async Task<string> SaveAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0) return string.Empty;

            var uploadsRoot = Path.Combine(_env.WebRootPath, folder);
            Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadsRoot, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{fileName}";
        }

        public Task DeleteAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return Task.CompletedTask;

            var full = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(full)) File.Delete(full);
            return Task.CompletedTask;
        }
    }
}
