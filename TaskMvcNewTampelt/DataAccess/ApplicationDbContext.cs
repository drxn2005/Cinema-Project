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

            // =========================
            // Actors <-> Movies (Many-to-Many) عبر MovieActor
            // =========================
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Category 1-* Movies
            // =========================
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

            // =========================
            // Movie 1-* Images
            // =========================
            modelBuilder.Entity<MovieImage>()
                .HasOne(mi => mi.Movie)
                .WithMany(m => m.Images)
                .HasForeignKey(mi => mi.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Cinema 1-* Halls
            // =========================
            modelBuilder.Entity<CinemaHall>()
                .HasOne(h => h.Cinema)
                .WithMany(c => c.Halls)
                .HasForeignKey(h => h.CinemaId)
                .OnDelete(DeleteBehavior.Restrict); 

            // =========================
            // Hall 1-* Seats
            // =========================
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.CinemaHall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.CinemaHallId)
                .OnDelete(DeleteBehavior.Restrict); 

            // =========================
            // SeatType 1-* Seats
            // =========================
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.SeatType)
                .WithMany(st => st.Seats)
                .HasForeignKey(s => s.SeatTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Movie 1-* Screenings    |   Hall 1-* Screenings
            // =========================
            modelBuilder.Entity<Screening>()
                .HasOne(sc => sc.Movie)
                .WithMany(m => m.Screenings)
                .HasForeignKey(sc => sc.MovieId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Screening>()
                .HasOne(sc => sc.CinemaHall)
                .WithMany(h => h.Screenings)
                .HasForeignKey(sc => sc.CinemaHallId)
                .OnDelete(DeleteBehavior.Restrict); 

            // =========================
            // Customer 1-* Bookings
            // =========================
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Screening 1-* Bookings   (: NO ACTION )
            // =========================
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Screening)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ScreeningId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Booking 1-* Tickets  
            // =========================
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Screening 1-* Tickets (NO ACTION  multiple cascade paths)
            // =========================
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Screening)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.ScreeningId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Seat 1-* Tickets (NO ACTION)
            // =========================
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets) 
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.ScreeningId, t.SeatId })
                .IsUnique();

            // =========================
            // Booking 1-1 Payment
            // =========================
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Screening>()
                .HasIndex(s => new { s.CinemaHallId, s.StartAt });

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Name);

            modelBuilder.Entity<CinemaHall>()
                .HasIndex(h => new { h.CinemaId, h.Name })
                .IsUnique(); 
        }

    }
}
