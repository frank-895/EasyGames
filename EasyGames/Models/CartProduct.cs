using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyGames.Models
{
    public enum ProductType
    {
        Book,
        Game,
        Toy
    }

    public class CartProduct
    {
        public int Id { get; set; } // PK

        [Required]
        public int CartId { get; set; } // FK

        [Required]
        public int ProductId { get; set; } // FK

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        // Another navigation property used for displaying product information from CartProduct object.
        public Product Product { get; set; } = null!;
    }
}