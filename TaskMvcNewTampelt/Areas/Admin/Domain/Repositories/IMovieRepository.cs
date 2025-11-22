using TaskMvcNewTampelt.Models;

namespace TaskMvcNewTampelt.Areas.Admin.Domain.Repositories
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetPagedAsync(int page, int pageSize, string? q);
        Task<Movie?> GetByIdAsync(int id, bool includeDetails = false);
        Task AddAsync(Movie movie);
        Task RemoveAsync(Movie movie);
        Task SaveAsync();
    }
}
