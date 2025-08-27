using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EasyGames.Models;
using EasyGames.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

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
            // Create roles and default users
            CreateRolesAndUsers(serviceProvider); 
            // Check if Books already exist
            if (!context.Book.Any())
            {
                context.Book.AddRange(
                    new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Classic", Price = 12.99m, StockQuantity = 10, ImageUrl = "/images/seeds/book-great-gatsby.jpg" },
                    new Book { Title = "1984", Author = "George Orwell", Genre = "Dystopian", Price = 9.99m, StockQuantity = 15, ImageUrl = "/images/seeds/book-1984.jpg" },
                    new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Classic", Price = 11.49m, StockQuantity = 12, ImageUrl = "/images/seeds/book-mockingbird.jpg" },
                    new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = "Classic", Price = 10.99m, StockQuantity = 8, ImageUrl = "/images/seeds/book-catcher-rye.jpg" },
                    new Book { Title = "Moby-Dick", Author = "Herman Melville", Genre = "Adventure", Price = 13.99m, StockQuantity = 6, ImageUrl = "/images/seeds/book-moby-dick.jpg" },
                    new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Genre = "Romance", Price = 8.99m, StockQuantity = 14, ImageUrl = "/images/seeds/book-pride-prejudice.jpg" },
                    new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Genre = "Fantasy", Price = 14.49m, StockQuantity = 9, ImageUrl = "/images/seeds/book-hobbit.jpg" },
                    new Book { Title = "Brave New World", Author = "Aldous Huxley", Genre = "Dystopian", Price = 10.49m, StockQuantity = 11, ImageUrl = "/images/seeds/book-brave-new-world.jpg" },
                    new Book { Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", Genre = "Fantasy", Price = 24.99m, StockQuantity = 7, ImageUrl = "/images/seeds/book-lotr.jpg" },
                    new Book { Title = "The Alchemist", Author = "Paulo Coelho", Genre = "Fiction", Price = 9.49m, StockQuantity = 13, ImageUrl = "/images/seeds/book-alchemist.jpg" }
                );
            }

            // Check if Games already exist
            if (!context.Game.Any())
            {
                context.Game.AddRange(
                    new Game { Title = "The Legend of Zelda", Platform = "Nintendo Switch", AgeRating = "E10+", Price = 59.99m, StockQuantity = 5, ImageUrl = "/images/seeds/game-zelda.jpg" },
                    new Game { Title = "Minecraft", Platform = "PC", AgeRating = "E", Price = 26.95m, StockQuantity = 20, ImageUrl = "/images/seeds/game-minecraft.jpg" },
                    new Game { Title = "Super Mario Odyssey", Platform = "Nintendo Switch", AgeRating = "E10+", Price = 49.99m, StockQuantity = 10, ImageUrl = "/images/seeds/game-mario-odyssey.jpg" },
                    new Game { Title = "God of War", Platform = "PS5", AgeRating = "M", Price = 69.99m, StockQuantity = 6, ImageUrl = "/images/seeds/game-god-of-war.jpg" },
                    new Game { Title = "Halo Infinite", Platform = "Xbox Series X", AgeRating = "T", Price = 59.99m, StockQuantity = 7, ImageUrl = "/images/seeds/game-halo-infinite.png" },
                    new Game { Title = "Elden Ring", Platform = "PS5", AgeRating = "M", Price = 59.99m, StockQuantity = 8, ImageUrl = "/images/seeds/game-elden-ring.jpg" },
                    new Game { Title = "Fortnite", Platform = "PC", AgeRating = "T", Price = 0.00m, StockQuantity = 100, ImageUrl = "/images/seeds/game-fortnite.jpeg" },
                    new Game { Title = "Animal Crossing: New Horizons", Platform = "Nintendo Switch", AgeRating = "E", Price = 59.99m, StockQuantity = 9, ImageUrl = "/images/seeds/game-animal-crossing.jpg" },
                    new Game { Title = "The Witcher 3", Platform = "PC", AgeRating = "M", Price = 39.99m, StockQuantity = 10, ImageUrl = "/images/seeds/game-witcher3.jpg" },
                    new Game { Title = "Stardew Valley", Platform = "PC", AgeRating = "E10+", Price = 14.99m, StockQuantity = 15, ImageUrl = "/images/seeds/game-stardew.png" }
                );
            }

            // Check if Toys already exist
            if (!context.Toy.Any())
            {
                context.Toy.AddRange(
                    new Toy { Name = "Lego Classic", RecommendedAge = "4+", Material = "Plastic", Price = 29.99m, StockQuantity = 25, ImageUrl = "/images/seeds/toy-lego.png" },
                    new Toy { Name = "Rubik\'s Cube", RecommendedAge = "8+", Material = "Plastic", Price = 14.99m, StockQuantity = 30, ImageUrl = "/images/seeds/toy-rubiks-cube.jpeg" },
                    new Toy { Name = "Barbie Doll", RecommendedAge = "3+", Material = "Plastic", Price = 19.99m, StockQuantity = 20, ImageUrl = "/images/seeds/toy-barbie.jpg" },
                    new Toy { Name = "Nerf Blaster", RecommendedAge = "8+", Material = "Plastic", Price = 24.99m, StockQuantity = 18, ImageUrl = "/images/seeds/toy-nerf.jpg" },
                    new Toy { Name = "Hot Wheels Car Set", RecommendedAge = "4+", Material = "Metal/Plastic", Price = 15.99m, StockQuantity = 35, ImageUrl = "/images/seeds/toy-hotwheels.jpg" },
                    new Toy { Name = "Teddy Bear", RecommendedAge = "3+", Material = "Plush", Price = 17.99m, StockQuantity = 16, ImageUrl = "/images/seeds/toy-teddy.jpg" },
                    new Toy { Name = "Jenga", RecommendedAge = "6+", Material = "Wood", Price = 16.99m, StockQuantity = 22, ImageUrl = "/images/seeds/toy-jenga.jpg" },
                    new Toy { Name = "Play-Doh", RecommendedAge = "3+", Material = "Compound", Price = 9.99m, StockQuantity = 28, ImageUrl = "/images/seeds/toy-playdoh.webp" },
                    new Toy { Name = "Remote Control Car", RecommendedAge = "6+", Material = "Plastic", Price = 34.99m, StockQuantity = 12, ImageUrl = "/images/seeds/toy-rc-car.jpeg" },
                    new Toy { Name = "UNO Card Game", RecommendedAge = "7+", Material = "Paper", Price = 7.99m, StockQuantity = 40, ImageUrl = "/images/seeds/toy-uno.jpg" }
                );
            }

            context.SaveChanges();
        }
    }

    private static void CreateRolesAndUsers(IServiceProvider serviceProvider)
    {
        // serviceProvider is used to for built-in services from ASP.NET
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Create roles if they don't exist
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
            }
        }

        // Create default admin user
        var adminEmail = "admin@easygames.com";
        var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(adminUser, "Admin123!").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").Wait();
            }
        }

        // Create default regular user
        var userEmail = "user@easygames.com";
        var regularUser = userManager.FindByEmailAsync(userEmail).Result;
        if (regularUser == null)
        {
            regularUser = new IdentityUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(regularUser, "User123!").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(regularUser, "User").Wait();
            }
        }
    }
}
