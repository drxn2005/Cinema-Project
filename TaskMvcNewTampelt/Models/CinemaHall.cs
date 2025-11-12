using System.ComponentModel.DataAnnotations;

namespace TaskMvcNewTampelt.Models
{
    public class CinemaHall
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = default!;

        // Seats + Screenings
        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
        public ICollection<Screening> Screenings { get; set; } = new HashSet<Screening>();
    }
}
