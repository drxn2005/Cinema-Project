using System.ComponentModel.DataAnnotations;

namespace TaskMvcNewTampelt.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [StringLength(255)]
        public string MainImg { get; set; } = string.Empty;

        // Many-to-many
        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();
    }
}
