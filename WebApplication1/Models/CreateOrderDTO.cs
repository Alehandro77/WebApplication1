namespace WebApplication1.Models
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
