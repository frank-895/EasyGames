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

        // this is a LLM-written workaround to avoid errors when running the application in Visual Studio (instead of VSC, where I've been working)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fix the key length issue for Identity tables
            builder.Entity<IdentityRole>().Property(e => e.Id).HasMaxLength(85);
            builder.Entity<IdentityUser>().Property(e => e.Id).HasMaxLength(85);
            builder.Entity<IdentityUserClaim<string>>().Property(e => e.UserId).HasMaxLength(85);
            builder.Entity<IdentityUserRole<string>>().Property(e => e.UserId).HasMaxLength(85);
            builder.Entity<IdentityUserRole<string>>().Property(e => e.RoleId).HasMaxLength(85);
            builder.Entity<IdentityUserLogin<string>>().Property(e => e.UserId).HasMaxLength(85);
            builder.Entity<IdentityUserToken<string>>().Property(e => e.UserId).HasMaxLength(85);
        }
    }
}
