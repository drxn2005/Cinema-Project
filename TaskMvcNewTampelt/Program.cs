using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskMvcNewTampelt.Areas.Admin.Domain.Repositories;
using TaskMvcNewTampelt.Data;
using TaskMvcNewTampelt.DataAccess;
using TaskMvcNewTampelt.Services;

namespace TaskMvcNewTampelt
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services
                        .AddIdentityCore<IdentityUser>(opt =>
                        {
                            opt.Password.RequiredLength = 6;
                            opt.Password.RequireNonAlphanumeric = false;
                            opt.User.RequireUniqueEmail = true;
                        })
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Repositories
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Services
            builder.Services.AddScoped<IMovieService, MovieService>();

            // Existing:
            builder.Services.AddScoped<IImageStorage, LocalImageStorage>();

            builder.Services.AddScoped<IImageStorage, LocalImageStorage>();

            var app = builder.Build();

            //_= SeedData.EnsureSeededAsync(app.Services);
           // await SeedData.EnsureSeededAsync(app.Services);
            await IdentitySeed.EnsureSeededAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
