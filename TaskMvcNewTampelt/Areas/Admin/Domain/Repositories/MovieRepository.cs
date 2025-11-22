// Infrastructure/Repositories/MovieRepository.cs
using Microsoft.EntityFrameworkCore;
using TaskMvcNewTampelt.Areas.Admin.Domain.Repositories;
using TaskMvcNewTampelt.DataAccess;
using TaskMvcNewTampelt.Models;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _db;
    public MovieRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<Movie>> GetPagedAsync(int page, int pageSize, string? q)
    {
        var query = _db.Movies.Include(m => m.Category).AsNoTracking().OrderByDescending(m => m.Id).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(m => m.Name.Contains(q));
        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public Task<Movie?> GetByIdAsync(int id, bool includeDetails = false)
    {
        IQueryable<Movie> q = _db.Movies;
        if (includeDetails)
            q = q.Include(m => m.Images)
                 .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor);
        return q.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(Movie movie) => await _db.Movies.AddAsync(movie);
    public Task RemoveAsync(Movie movie) { _db.Movies.Remove(movie); return Task.CompletedTask; }
    public Task SaveAsync() => _db.SaveChangesAsync();
}
