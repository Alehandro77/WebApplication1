namespace WebApplication1.Models
{
    public class CartItemDTO
    {
        public int IdCartItems { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
