using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMvcNewTampelt.Models
{
    public enum PaymentStatus { Pending = 0, Succeeded = 1, Failed = 2, Refunded = 3 }

    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = default!;

        [Column(TypeName = "decimal(12,2)")]
        public decimal Amount { get; set; }

        [StringLength(30)]
        public string Method { get; set; } = "Cash"; //or Card/Online...

        public DateTime? PaidAt { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}
