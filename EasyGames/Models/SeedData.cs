using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EasyGames.Models;
using EasyGames.Data;
using System;
using System.Linq;

namespace EasyGames.Models;

// note - m = decimal instead of default double
// good for money/currency - especially as we defined our price as a decimal value

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            // Check if Books already exist
            if (!context.Book.Any())
            {
                context.Book.AddRange(
                    new Book
                    {
                        Title = "The Great Gatsby",
                        Author = "F. Scott Fitzgerald",
                        Genre = "Classic",
                        Price = 12.99m,
                        StockQuantity = 10
                    },
                    new Book
                    {
                        Title = "1984",
                        Author = "George Orwell",
                        Genre = "Dystopian",
                        Price = 9.99m,
                        StockQuantity = 15
                    }
                );
            }

            // Check if Games already exist
            if (!context.Game.Any())
            {
                context.Game.AddRange(
                    new Game
                    {
                        Title = "The Legend of Zelda",
                        Platform = "Nintendo Switch",
                        AgeRating = "E10+",
                        Price = 59.99m,
                        StockQuantity = 5
                    },
                    new Game
                    {
                        Title = "Minecraft",
                        Platform = "PC",
                        AgeRating = "E",
                        Price = 26.95m,
                        StockQuantity = 20
                    }
                );
            }

            // Check if Toys already exist
            if (!context.Toy.Any())
            {
                context.Toy.AddRange(
                    new Toy
                    {
                        Name = "Lego Classic",
                        RecommendedAge = "4+",
                        Material = "Plastic",
                        Price = 29.99m,
                        StockQuantity = 25
                    },
                    new Toy
                    {
                        Name = "Rubik's Cube",
                        RecommendedAge = "8+",
                        Material = "Plastic",
                        Price = 14.99m,
                        StockQuantity = 30
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
