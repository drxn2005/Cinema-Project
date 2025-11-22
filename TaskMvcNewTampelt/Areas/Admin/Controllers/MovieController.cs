using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using TaskMvcNewTampelt.Areas.Admin.Domain;
using TaskMvcNewTampelt.Areas.Admin.Domain.Repositories;
using TaskMvcNewTampelt.DataAccess;
using TaskMvcNewTampelt.Models;
using TaskMvcNewTampelt.Services;

namespace TaskMvcNewTampelt.Areas.Admin.Controllers
{
    //app.UseAuthentication();
    //app.UseAuthorization();

    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieService _service;
        private readonly IActorRepository _actorRepo;
        private readonly ICategoryRepository _categoryRepo;

        public MovieController(IMovieService service, IActorRepository actorRepo, ICategoryRepository categoryRepo)
        { _service = service; _actorRepo = actorRepo; _categoryRepo = categoryRepo; }

        public async Task<IActionResult> Index(string? q, int page = 1, int pageSize = 10)
            => View(await _service.ListAsync(page, pageSize, q));

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillLookups();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieCreateDto dto)
        {
            if (!ModelState.IsValid) { await FillLookups(); return View(dto); }
            await _service.CreateAsync(dto);
            TempData["ok"] = "تم إضافة الفيلم";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _service.GetAsync(id, details: true);
            if (movie == null) return NotFound();
            await FillLookups(movie.MovieActors.Select(ma => ma.ActorId).ToArray());
            var dto = new MovieEditDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Status = movie.Status,
                ReleaseDate = movie.ReleaseDate,
                CategoryId = movie.CategoryId,
                ExistingMainImg = movie.MainImg
            };
            return View(dto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieEditDto dto)
        {
            if (!ModelState.IsValid) { await FillLookups(); return View(dto); }
            await _service.UpdateAsync(dto);
            TempData["ok"] = "تم تعديل الفيلم";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["ok"] = "تم الحذف";
            return RedirectToAction(nameof(Index));
        }

        private async Task FillLookups(int[]? selected = null)
        {
            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
            ViewBag.Actors = new MultiSelectList(await _actorRepo.GetAllAsync(), "Id", "Name", selected);
        }
    }

}
