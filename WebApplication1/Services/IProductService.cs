using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface IProductService
    {
        Task<(List<ProductDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<ProductDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<ProductDTO> CreateAsync(CreateProductDTO dto, CancellationToken ct);
        Task<ProductDTO?> UpdateAsync(int id, UpdateProductDTO dto, CancellationToken ct); 
        Task<ProductDTO?> PatсhAsync(int id, PatchProductDTO dto, CancellationToken ct); 
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
