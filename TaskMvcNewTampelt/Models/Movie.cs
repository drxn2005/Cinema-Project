using System.ComponentModel.DataAnnotations;

namespace TaskMvcNewTampelt.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        public DateTime ReleaseDate { get; set; }

        [StringLength(255)]
        public string MainImg { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public ICollection<MovieImage> Images { get; set; } = new HashSet<MovieImage>();
        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();
        public ICollection<Screening> Screenings { get; set; } = new HashSet<Screening>();
    }
}
