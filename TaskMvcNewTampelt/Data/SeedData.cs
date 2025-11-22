using Microsoft.EntityFrameworkCore;
using TaskMvcNewTampelt.DataAccess;
using TaskMvcNewTampelt.Models;

namespace TaskMvcNewTampelt.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeededAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync(); 

            // Categories
            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    new Category { Name = "Action", Status = true, Description = "أكشن" },
                    new Category { Name = "Comedy", Status = true, Description = "كوميدي" },
                    new Category { Name = "Drama", Status = true, Description = "دراما" }
                );
                await db.SaveChangesAsync();
            }

            // Actors
            if (!await db.Actors.AnyAsync())
            {
                db.Actors.AddRange(
                    new Actor { Name = "Tom Hanks", Status = true, MainImg = "/img/actors/tom.jpg" },
                    new Actor { Name = "Scarlett Johansson", Status = true, MainImg = "/img/actors/scarlett.jpg" },
                    new Actor { Name = "Denzel Washington", Status = true, MainImg = "/img/actors/denzel.jpg" }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.Cinemas.AnyAsync())
            {
                var cinema = new Cinema { Name = "Downtown Cinema", Status = true, MainImg = "/img/cinemas/1.jpg" };
                db.Cinemas.Add(cinema);
                await db.SaveChangesAsync();

                var hall = new CinemaHall { Name = "Hall 1", CinemaId = cinema.Id };
                db.CinemaHalls.Add(hall);
                await db.SaveChangesAsync();

                if (!await db.SeatTypes.AnyAsync())
                {
                    db.SeatTypes.AddRange(
                        new SeatType { Name = "Standard", PriceFactor = 1.00m },
                        new SeatType { Name = "VIP", PriceFactor = 1.50m }
                    );
                    await db.SaveChangesAsync();
                }

                var std = await db.SeatTypes.FirstAsync(st => st.Name == "Standard");
                var seats = new List<Seat>();
                for (char row = 'A'; row <= 'C'; row++)
                {
                    for (int n = 1; n <= 6; n++)
                        seats.Add(new Seat { CinemaHallId = hall.Id, RowLabel = row.ToString(), SeatNumber = n, SeatTypeId = std.Id, IsActive = true });
                }
                db.Seats.AddRange(seats);
                await db.SaveChangesAsync();
            }
        }
    }
}
