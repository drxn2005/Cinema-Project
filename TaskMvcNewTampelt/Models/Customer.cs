using System.ComponentModel.DataAnnotations;

namespace TaskMvcNewTampelt.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Phone, StringLength(50)]
        public string? Phone { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
