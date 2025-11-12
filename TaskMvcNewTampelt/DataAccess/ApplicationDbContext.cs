using Microsoft.EntityFrameworkCore;
using TaskMvcNewTampelt.Models;

namespace TaskMvcNewTampelt.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Cinema> Cinemas => Set<Cinema>();
        public DbSet<CinemaHall> CinemaHalls => Set<CinemaHall>();
        public DbSet<SeatType> SeatTypes => Set<SeatType>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<MovieImage> MovieImages => Set<MovieImage>();
        public DbSet<MovieActor> MovieActors => Set<MovieActor>();
        public DbSet<Screening> Screenings => Set<Screening>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=.;Database=TaskCenima;Trusted_Connection=True;Encrypt=True;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Movie <-> Actor (many-to-many via explicit join)
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId);

            // MovieImage
            modelBuilder.Entity<MovieImage>()
                .HasOne(mi => mi.Movie)
                .WithMany(m => m.Images)
                .HasForeignKey(mi => mi.MovieId);

            // Cinema -> Halls -> Seats
            modelBuilder.Entity<CinemaHall>()
                .HasOne(h => h.Cinema)
                .WithMany(c => c.Halls)
                .HasForeignKey(h => h.CinemaId);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.CinemaHall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.CinemaHallId);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.SeatType)
                .WithMany(st => st.Seats)
                .HasForeignKey(s => s.SeatTypeId);

            // Screening
            modelBuilder.Entity<Screening>()
                .HasOne(sc => sc.Movie)
                .WithMany(m => m.Screenings)
                .HasForeignKey(sc => sc.MovieId);

            modelBuilder.Entity<Screening>()
                .HasOne(sc => sc.CinemaHall)
                .WithMany(h => h.Screenings)
                .HasForeignKey(sc => sc.CinemaHallId);

            // Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Screening)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ScreeningId);

            // Ticket (unique seat per screening)
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.ScreeningId, t.SeatId })
                .IsUnique();

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Screening)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.ScreeningId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId);

            // Payment 1-1 Booking
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);
        }
    
}
}
