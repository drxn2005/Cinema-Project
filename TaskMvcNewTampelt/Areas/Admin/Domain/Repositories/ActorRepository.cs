// Infrastructure/Repositories/ActorRepository.cs
using Microsoft.EntityFrameworkCore;
using TaskMvcNewTampelt.Areas.Admin.Domain.Repositories;
using TaskMvcNewTampelt.DataAccess;
using TaskMvcNewTampelt.Models;

public class ActorRepository : IActorRepository
{
    private readonly ApplicationDbContext _db;
    public ActorRepository(ApplicationDbContext db) => _db = db;

    public Task<List<Actor>> GetAllAsync() => _db.Actors.OrderBy(a => a.Name).ToListAsync();
}

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) => _db = db;

    public Task<List<Category>> GetAllAsync() => _db.Categories.OrderBy(c => c.Name).ToListAsync();
}
