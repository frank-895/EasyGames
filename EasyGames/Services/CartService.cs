using EasyGames.Data;
using EasyGames.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Services
{
    public class CartService : ICartService // declares that CartService is implementing ICartService
    // we use a service to separate business logic from controllers
    {
        private readonly ApplicationDbContext _context;
        // the instructor version used IHttpContextAccessor to get request-specific informaiton (like the User)
        // by explictly passing userId instead (which we can do because our model requires it, the service is more reusable
        // its also just a bit cleaner and understandable (arguably)

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Cart GetOrCreateUserCart(string userId)
        // returns a cart for userId (or creates a new one if no cart exists)
        {
            var cart = _context.Carts // here we are using those useful navigation properties 
                .Include(c => c.CartProducts) // no need for messy manual joins
                .ThenInclude(cp => cp.Product) // ThenInclude uses the navigation property of the navigation property! 
                .FirstOrDefault(c => c.UserId == userId); // retrieves first Cart or null if no Cart exists for user

            if (cart == null)
            {
                cart = new Cart { UserId = userId }; // create a new Cart object
                _context.Carts.Add(cart); // add it to the DB
                _context.SaveChanges();
            }

            return cart;
        }

        public void AddToCart(string userId, int productId, ProductType productType, int quantity = 1)
        // adds a product to the user's cart, with a default quantity of 1
        {
            var cart = GetOrCreateUserCart(userId); // ensures every user has a cart

            var existingCartProduct = cart.CartProducts
                .FirstOrDefault(cp => cp.ProductId == productId && cp.ProductType == productType);
                // check if this product is the first product in the cart

            if (existingCartProduct != null)
            {
                existingCartProduct.Quantity += quantity; // if this product already exists just add the quantity
            }
            else
            {
                // otherwise, the product needs to be added, requiring a new CartProduct object
                var cartProduct = new CartProduct
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    ProductType = productType,
                    Quantity = quantity
                };
                _context.CartProducts.Add(cartProduct);
            }

            _context.SaveChanges();
        }

        public void RemoveFromCart(string userId, int productId, ProductType productType)
        // this completely removes the product from the cart (instructor's method also decreases quantity, we have a separate method for that)
        {
            var cart = GetOrCreateUserCart(userId); // again, ensures user has a cart

            var cartProduct = cart.CartProducts
                .FirstOrDefault(cp => cp.ProductId == productId && cp.ProductType == productType);

            if (cartProduct != null) // makes sure the product exists in the cart before trying to delete it (extra safe)
            {
                _context.CartProducts.Remove(cartProduct);
                _context.SaveChanges();
            }
        }

        public void UpdateQuantity(string userId, int productId, ProductType productType, int quantity)
        // change the quantity (separate method to removing product entirely)
        {
            if (quantity <= 0)
            {
                RemoveFromCart(userId, productId, productType); // if the quantity is negative, assume user wants product deleted.
                return;
            }

            var cart = GetOrCreateUserCart(userId); // check cart exists (RemoveFromCart already checks this)

            var cartProduct = cart.CartProducts
                .FirstOrDefault(cp => cp.ProductId == productId && cp.ProductType == productType);

            if (cartProduct != null) // if the product exists in the cart, update the quantity
            {
                cartProduct.Quantity = quantity;
                _context.SaveChanges();
            }
        }

        public void ClearCart(string userId)
        // this method removes every item from the user's cart
        {
            var cart = _context.Carts
                .Include(c => c.CartProducts) // this fetches BOTH the cart AND the associated products
                .FirstOrDefault(c => c.UserId == userId);

            if (cart != null) // as long as the cart exists
            {
                _context.CartProducts.RemoveRange(cart.CartProducts); // remove all products with builtin RemoveRange method
                _context.SaveChanges();
            }
        }

        public int GetCartItemCount(string userId)
        // retrieves the total number of items in the user's cart
        {
            var cart = _context.Carts
                .Include(c => c.CartProducts)
                .FirstOrDefault(c => c.UserId == userId);

            // this line was compacted by ChatGPT - cool syntax! Let me explain:
            // cart? checks if cart is null
            // .CartProducts accesses the CartProduct objects via the navigation property
            // Sum is a LINQ metho to add up all values 
            // ?? 0 returns 0 in case the cart is null
            return cart?.CartProducts.Sum(cp => cp.Quantity) ?? 0;
        }
    }
}