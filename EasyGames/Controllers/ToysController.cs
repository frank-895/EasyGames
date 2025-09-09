using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyGames.Data;
using EasyGames.Models;
using Microsoft.AspNetCore.Authorization;

namespace EasyGames.Controllers
{
    public class ToysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Toys
        public async Task<IActionResult> Index()
        {
            return View(await _context.Toy.ToListAsync());
        }

        // GET: Toys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toy = await _context.Toy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toy == null)
            {
                return NotFound();
            }

            return View(toy);
        }

        // GET: Toys/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Toys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,RecommendedAge,Material,Id,Price,StockQuantity")] Toy toy, IFormFile? imageFile)
        {
            // Image is optional
            ModelState.Remove("ImageUrl");
            ModelState.Remove("imageFile");

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    toy.ImageUrl = "/images/products/" + fileName;
                }

                _context.Add(toy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toy);
        }

        // GET: Toys/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toy = await _context.Toy.FindAsync(id);
            if (toy == null)
            {
                return NotFound();
            }
            return View(toy);
        }

        // POST: Toys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,RecommendedAge,Material,Id,Price,StockQuantity")] Toy toy, IFormFile? imageFile)
        {
            if (id != toy.Id)
            {
                return NotFound();
            }

            // Image is optional
            ModelState.Remove("ImageUrl");
            ModelState.Remove("imageFile");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingToy = await _context.Toy.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (existingToy == null)
                    {
                        return NotFound();
                    }

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                        Directory.CreateDirectory(uploadsFolder);

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrEmpty(existingToy.ImageUrl))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingToy.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        toy.ImageUrl = "/images/products/" + fileName;
                    }
                    else
                    {
                        toy.ImageUrl = existingToy.ImageUrl;
                    }

                    _context.Update(toy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToyExists(toy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toy);
        }

        // GET: Toys/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toy = await _context.Toy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toy == null)
            {
                return NotFound();
            }

            return View(toy);
        }

        // POST: Toys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toy = await _context.Toy.FindAsync(id);
            if (toy != null)
            {
                _context.Toy.Remove(toy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToyExists(int id)
        {
            return _context.Toy.Any(e => e.Id == id);
        }
    }
}
