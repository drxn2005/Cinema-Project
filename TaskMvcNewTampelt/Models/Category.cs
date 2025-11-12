using System.ComponentModel.DataAnnotations;

namespace TaskMvcNewTampelt.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
