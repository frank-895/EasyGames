using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EasyGames.Models;
using Microsoft.AspNetCore.Identity;

namespace EasyGames.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EasyGames.Models.Book> Book { get; set; } = default!;
        public DbSet<EasyGames.Models.Game> Game { get; set; } = default!;
        public DbSet<EasyGames.Models.Toy> Toy { get; set; } = default!;
        public DbSet<EasyGames.Models.Cart> Carts { get; set; } = default!;
        public DbSet<EasyGames.Models.CartProduct> CartProducts { get; set; } = default!;
    }
}
