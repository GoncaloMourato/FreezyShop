namespace FreezyShop.Data.Entities
{
    public class CartItem : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string ImageUrl1 { get; set; }
        public decimal TotalPrice { get { return Quantity * Price; } }

        public string UserId { get; set; }

        
        public User User { get; set; }

    }
}
