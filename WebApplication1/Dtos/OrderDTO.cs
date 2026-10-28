namespace WebApplication1.Dtos
{
    public class OrderDTO
    {
        public int IdOrders { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
