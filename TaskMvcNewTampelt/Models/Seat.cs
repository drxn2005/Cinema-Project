using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TaskMvcNewTampelt.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public int CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; } = default!;

        [Required, StringLength(5)]
        public string RowLabel { get; set; } = "A";

        public int SeatNumber { get; set; }

        public int SeatTypeId { get; set; }
        public SeatType SeatType { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
