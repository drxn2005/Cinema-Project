using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TaskMvcNewTampelt.Models
{
    public class Screening
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = default!;

        public int CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; } = default!;

        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        [StringLength(20)]
        public string Format { get; set; } = "2D"; 
        [StringLength(10)]
        public string Language { get; set; } = "EN";

        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePrice { get; set; } = 100;

        public bool IsActive { get; set; } = true;

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
