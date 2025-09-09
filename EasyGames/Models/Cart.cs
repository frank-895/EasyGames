using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyGames.Models
{
    public class Cart
    {
        public int Id { get; set; } // PK

        [Required]
        public string UserId { get; set; } = string.Empty; // FK

        // This is a navigation property. 
        // Navigation properties are kind of like FKs except they have no impact on the DB.
        // They are simply used to allow you to navigate relationships with code.
        // This particular property lets you access the CartProduct objects from the Cart object.
        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}