using System.Diagnostics;
using EasyGames.Models;
using Microsoft.AspNetCore.Mvc;
using EasyGames.Data;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get the 10 products with the highest stock levels
            var popularProducts = await GetPopularProductsAsync(); // call the function, return the results to the view
            return View(popularProducts);
        }

        private async Task<List<object>> GetPopularProductsAsync()
        {
            // Get all products and combine them - ChatGPT helped me write these queries
            var books = await _context.Book
                .OrderByDescending(b => b.StockQuantity)
                .Take(9) // we take 10 - at most, there can be 10 books with the highest stock quantity
                .ToListAsync();

            var games = await _context.Game
                .OrderByDescending(g => g.StockQuantity)
                .Take(9) // we take 9 - at most, there can be 9 games with the highest stock quantity
                .ToListAsync();

            var toys = await _context.Toy
                .OrderByDescending(t => t.StockQuantity)
                .Take(9) // we take 9 - at most, there can be 9 toys with the highest stock quantity
                .ToListAsync();

            // Combine all products and order by stock quantity
            var allProducts = books.Cast<object>()
                .Concat(games.Cast<object>())
                .Concat(toys.Cast<object>())
                .OrderByDescending(p => GetStockQuantity(p))
                .Take(9) // we only take the top 9 products
                .ToList();

            return allProducts;
        }

        private int GetStockQuantity(object product)
        {
            // Use reflection to get StockQuantity property - ChatGPT taught me about reflection
            // Its used to get the properties of the model at runtime, allowing us to mix and match models.
            var property = product.GetType().GetProperty("StockQuantity");
            return property != null ? (int)(property.GetValue(product) ?? 0) : 0;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
