using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EasyGames.Services;
using EasyGames.Models;
using System.Security.Claims;

namespace EasyGames.Controllers
{
    [Authorize] // protect the whole controller (like UserManagementController) so only authenticated users can have cart functionality
    // this is necessary because our current implementation ties each Cart to a User.
    public class CartController : Controller
    {
        private readonly ICartService _cartService; // holds the injected service

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        // Because we are using explicit UserIds in the CartService we need to understand claims.
        // A claim is a key-value pair containing information about a user. 
        // NameIdentifier is a predefined constant defined in System.Security.Claims
        // This is how we retrieve the UserId
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _cartService.GetOrCreateUserCart(userId);
            return View(cart);
        }

        
        [HttpPost]
        public IActionResult AddToCart(int productId, ProductType productType, int quantity = 1)
        {
            // action adds to cart, only on POST request
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.AddToCart(userId, productId, productType, quantity);
            
            return RedirectToAction("Index"); // redirect to cart page
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, ProductType productType, int quantity)
        {
            // updates quantity, only on POST request
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.UpdateQuantity(userId, productId, productType, quantity);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId, ProductType productType)
        {
            // removes product, only on POST request
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.RemoveFromCart(userId, productId, productType);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            // clears entire cart, only on POST request
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.ClearCart(userId);
            
            return RedirectToAction("Index");
        }
    }
}