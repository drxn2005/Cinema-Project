using TaskMvcNewTampelt.Models;

namespace TaskMvcNewTampelt.Areas.Admin.Domain.Repositories
{
    public interface IMovieService
    {
        Task<List<Movie>> ListAsync(int page, int pageSize, string? q);
        Task<Movie?> GetAsync(int id, bool details = false);
        Task<int> CreateAsync(MovieCreateDto dto);
        Task UpdateAsync(MovieEditDto dto);
        Task DeleteAsync(int id);
    }

}
