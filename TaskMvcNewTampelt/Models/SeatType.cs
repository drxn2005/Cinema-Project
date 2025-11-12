using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMvcNewTampelt.Models
{
    public class SeatType
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = "Standard";

        [Column(TypeName = "decimal(5,2)")]
        public decimal PriceFactor { get; set; } = 1.00m;

        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
