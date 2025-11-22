using TaskMvcNewTampelt.Areas.Admin.Domain;
using TaskMvcNewTampelt.Areas.Admin.Domain.Repositories;
using TaskMvcNewTampelt.Models;
using TaskMvcNewTampelt.Services; // IImageStorage

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movies;
    private readonly IActorRepository _actors;
    private readonly ICategoryRepository _categories;
    private readonly IImageStorage _images;

    public MovieService(IMovieRepository m, IActorRepository a, ICategoryRepository c, IImageStorage img)
    { _movies = m; _actors = a; _categories = c; _images = img; }

    public Task<List<Movie>> ListAsync(int page, int pageSize, string? q)
        => _movies.GetPagedAsync(page, pageSize, q);

    public Task<Movie?> GetAsync(int id, bool details = false)
        => _movies.GetByIdAsync(id, details);

    public async Task<int> CreateAsync(MovieCreateDto dto)
    {
        var movie = new Movie
        {
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            ReleaseDate = dto.ReleaseDate,
            CategoryId = dto.CategoryId
        };

        if (dto.MainImage != null)
            movie.MainImg = await _images.SaveAsync(dto.MainImage, "uploads/movies");

        if (dto.SubImages?.Any() == true)
        {
            movie.Images = new List<MovieImage>();
            foreach (var f in dto.SubImages)
            {
                var path = await _images.SaveAsync(f, "uploads/movies");
                if (!string.IsNullOrWhiteSpace(path))
                    movie.Images.Add(new MovieImage { Img = path });
            }
        }

        if (dto.ActorIds?.Any() == true)
            movie.MovieActors = dto.ActorIds.Select(id => new MovieActor { ActorId = id }).ToList();

        await _movies.AddAsync(movie);
        await _movies.SaveAsync();
        return movie.Id;
    }

    public async Task UpdateAsync(MovieEditDto dto)
    {
        var existing = await _movies.GetByIdAsync(dto.Id, includeDetails: true)
                      ?? throw new KeyNotFoundException("Movie not found");

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Status = dto.Status;
        existing.ReleaseDate = dto.ReleaseDate;
        existing.CategoryId = dto.CategoryId;

        if (dto.MainImage != null)
        {
            await _images.DeleteAsync(existing.MainImg);
            existing.MainImg = await _images.SaveAsync(dto.MainImage, "uploads/movies");
        }

        if (dto.SubImages?.Any() == true)
            foreach (var f in dto.SubImages)
            {
                var path = await _images.SaveAsync(f, "uploads/movies");
                if (!string.IsNullOrWhiteSpace(path))
                    existing.Images.Add(new MovieImage { Img = path });
            }

        existing.MovieActors = (dto.ActorIds ?? Array.Empty<int>())
                               .Select(a => new MovieActor { MovieId = existing.Id, ActorId = a }).ToList();

        await _movies.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await _movies.GetByIdAsync(id, includeDetails: true)
                    ?? throw new KeyNotFoundException("Movie not found");

        if (movie.Screenings.Any())
            throw new InvalidOperationException("لا يمكن حذف فيلم له عروض.");

        foreach (var img in movie.Images)
            await _images.DeleteAsync(img.Img);

        await _movies.RemoveAsync(movie);
        await _movies.SaveAsync();
    }
}
