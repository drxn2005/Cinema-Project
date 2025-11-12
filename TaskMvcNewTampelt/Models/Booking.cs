using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TaskMvcNewTampelt.Models
{
    public enum BookingStatus { Pending = 0, Paid = 1, Cancelled = 2 }

    public class Booking
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public int ScreeningId { get; set; }
        public Screening Screening { get; set; } = default!;

        public DateTime BookedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public Payment? Payment { get; set; }
    }
}
