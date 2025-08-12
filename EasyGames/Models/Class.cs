using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// NOTE - ? makes the field nullable

namespace EasyGames.Models
{
    public abstract class Product
    {
        public int Id { get; set; }

        [Required]
        [Precision(18, 2)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }

    public class Book : Product
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Author { get; set; }

        [MaxLength(100)]
        public string? Genre { get; set; }
    }

    public class Game : Product
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Platform { get; set; }

        [MaxLength(100)]
        public string? AgeRating { get; set; }
    }

    public class Toy : Product
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? RecommendedAge { get; set; }

        [MaxLength(100)]
        public string? Material { get; set; }
    }
}
