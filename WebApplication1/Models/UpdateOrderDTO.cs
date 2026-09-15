namespace WebApplication1.Models
{
    public class UpdateOrderDTO
    {
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
