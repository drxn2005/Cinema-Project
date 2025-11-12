using System.ComponentModel.DataAnnotations;

namespace TaskMvcNewTampelt.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [StringLength(255)]
        public string MainImg { get; set; } = string.Empty;

        public ICollection<CinemaHall> Halls { get; set; } = new HashSet<CinemaHall>();
    }
}
