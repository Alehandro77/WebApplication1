namespace WebApplication1.Dtos
{
    public class OrderItemDTO
    {
        public int IdOrderItem { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
