using EasyGames.Models;

// we put cart service in services/ despite the instructor version putting in models/
// This is better separation of concerns
// The ICartService does not define a model but a service contract (or interface!)
namespace EasyGames.Services
{
    public interface ICartService
    {
        // The interface defines the methods needed in the CartService. 
        // NO LOGIC contained here. 
        Cart GetOrCreateUserCart(string userId);
        void AddToCart(string userId, int productId, ProductType productType, int quantity = 1);
        void RemoveFromCart(string userId, int productId, ProductType productType);
        void UpdateQuantity(string userId, int productId, ProductType productType, int quantity);
        void ClearCart(string userId);
        int GetCartItemCount(string userId);
    }
}