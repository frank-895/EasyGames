using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EasyGames.Models;

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
    }
}
