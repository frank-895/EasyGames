namespace EasyGames.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class PopularProductsViewModel
    // This is a view model for the home page. It is used to display the popular products.
    {
        public List<Book> Books { get; set; } = new();
        public List<Game> Games { get; set; } = new();
        public List<Toy> Toys { get; set; } = new();
    }
}
