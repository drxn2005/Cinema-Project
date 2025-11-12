using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMvcNewTampelt.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = default!;

        public int ScreeningId { get; set; }
        public Screening Screening { get; set; } = default!;

        public int SeatId { get; set; }
        public Seat Seat { get; set; } = default!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required, StringLength(30)]
        public string Code { get; set; } = Guid.NewGuid().ToString("N")[..10].ToUpper();
    }
}
