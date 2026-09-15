namespace WebApplication1.Models
{
    public class PatchOrderItemDTO
    {
        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
