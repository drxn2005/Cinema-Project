using TaskMvcNewTampelt.Models;

namespace TaskMvcNewTampelt.Areas.Admin.Domain.Repositories
{
    public interface IActorRepository
    {
        Task<List<Actor>> GetAllAsync();
    }
}
