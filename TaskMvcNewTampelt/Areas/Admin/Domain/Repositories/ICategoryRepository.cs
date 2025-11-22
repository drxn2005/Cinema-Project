using TaskMvcNewTampelt.Models;

namespace TaskMvcNewTampelt.Areas.Admin.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}
