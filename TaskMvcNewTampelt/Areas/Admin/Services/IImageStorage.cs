using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TaskMvcNewTampelt.Services
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(IFormFile file, string folder); 
        Task DeleteAsync(string relativePath);                 
    }
}
