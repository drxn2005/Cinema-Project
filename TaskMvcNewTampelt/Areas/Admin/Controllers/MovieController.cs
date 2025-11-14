using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using TaskMvcNewTampelt.DataAccess;
using TaskMvcNewTampelt.Models;
using TaskMvcNewTampelt.Services;

namespace TaskMvcNewTampelt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IImageStorage _images;

        public MovieController(ApplicationDbContext db, IImageStorage images)
        {
            _db = db;
            _images = images;
        }

        // GET: Admin/Movie
        public async Task<IActionResult> Index()
        {
            var movies = await _db.Movies
                .Include(m => m.Category)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            return View(movies);
        }

        // ========== CREATE ==========
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillLookups();
            return View(new Movie { ReleaseDate = DateTime.Today, Status = true });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Status,ReleaseDate,CategoryId")] Movie movie,
            IFormFile? mainImage,
            List<IFormFile>? subImages,
            int[]? actorIds)
        {
            if (!ModelState.IsValid)
            {
                await FillLookups();
                return View(movie);
            }

            if (mainImage != null)
                movie.MainImg = await _images.SaveAsync(mainImage, "uploads/movies");

            if (subImages?.Any() == true)
            {
                movie.Images = new List<MovieImage>();
                foreach (var f in subImages)
                {
                    var path = await _images.SaveAsync(f, "uploads/movies");
                    if (!string.IsNullOrWhiteSpace(path))
                        movie.Images.Add(new MovieImage { Img = path });
                }
            }

            if (actorIds?.Any() == true)
                movie.MovieActors = actorIds.Select(id => new MovieActor { ActorId = id }).ToList();

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();
            TempData["ok"] = "تم إضافة الفيلم";
            return RedirectToAction(nameof(Index));
        }

        // ========== EDIT ==========
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _db.Movies
                .Include(m => m.Images)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            await FillLookups(selectedActors: movie.MovieActors.Select(ma => ma.ActorId).ToArray());
            return View(movie);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description,Status,ReleaseDate,CategoryId,MainImg")] Movie movie, // MainImg 
            IFormFile? mainImage,
            List<IFormFile>? subImages,
            int[]? actorIds)
        {
            var existing = await _db.Movies
                .Include(m => m.Images)
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existing == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await FillLookups(selectedActors: existing.MovieActors.Select(ma => ma.ActorId).ToArray());
                return View(existing);
            }

            existing.Name = movie.Name;
            existing.Description = movie.Description;
            existing.Status = movie.Status;
            existing.ReleaseDate = movie.ReleaseDate;
            existing.CategoryId = movie.CategoryId;

            if (mainImage != null)
            {
                await _images.DeleteAsync(existing.MainImg);
                existing.MainImg = await _images.SaveAsync(mainImage, "uploads/movies");
            }

            if (subImages?.Any() == true)
            {
                foreach (var f in subImages)
                {
                    var path = await _images.SaveAsync(f, "uploads/movies");
                    if (!string.IsNullOrWhiteSpace(path))
                        existing.Images.Add(new MovieImage { Img = path });
                }
            }

            existing.MovieActors = (actorIds ?? Array.Empty<int>())
                .Select(aid => new MovieActor { MovieId = existing.Id, ActorId = aid })
                .ToList();

            await _db.SaveChangesAsync();
            TempData["ok"] = "تم تعديل الفيلم";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var img = await _db.MovieImages.FindAsync(id);
            if (img == null) return NotFound();
            await _images.DeleteAsync(img.Img);
            _db.MovieImages.Remove(img);
            await _db.SaveChangesAsync();
            return Ok();
        }


        // ========== DETAILS ==========
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _db.Movies.AsNoTracking()
                .Include(m => m.Category)
                .Include(m => m.Images)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie == null ? NotFound() : View(movie);
        }

        // ========== DELETE ==========
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _db.Movies
                .Include(m => m.Screenings)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();
            if (movie.Screenings.Any())
            {
                TempData["err"] = "لا يمكن حذف فيلم له عروض. عطّل الحالة فقط.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var img in _db.MovieImages.Where(i => i.MovieId == id))
                await _images.DeleteAsync(img.Img);

            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync();
            TempData["ok"] = "تم الحذف";
            return RedirectToAction(nameof(Index));
        }

        // Helpers Methods
        private async Task FillLookups(int[]? selectedActors = null)
        {
            ViewBag.Categories = new SelectList(
                await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");

            var actors = await _db.Actors.OrderBy(a => a.Name).ToListAsync();
            ViewBag.Actors = new MultiSelectList(actors, "Id", "Name", selectedActors);
        }
    }
}
