namespace WebApplication1.Models
{
    public class PatchProductDTO
    {
        public string? Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        public string? Url { get; set; }
    }
}
