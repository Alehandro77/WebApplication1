using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<ProductDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var products = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<ProductDTO>>(products), total);
        }

        public async Task<ProductDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var product = await _repository.GetByIdAsync(id, ct);
            return product is null ? null : _mapper.Map<ProductDTO?>(product);
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO dto, CancellationToken ct)
        {
            _ = await _repository.FintCategoryAsync(dto.CategoryId, ct)
                ?? throw new InvalidOperationException(
                    $"Категория с Id {dto.CategoryId} не найдена.");

            var product = _mapper.Map<Product>(dto);
            
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(product.IdProducts, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданный продукт.");
            return _mapper.Map<ProductDTO>(saved);
        }

        public async Task<ProductDTO?> UpdateAsync(
            int id, UpdateProductDTO dto, CancellationToken ct)
        {
            var product = await _repository.GetByIdAsync(id, ct);
            if (product is null) return null;

            _ = await _repository.FintCategoryAsync(dto.CategoryId, ct)
                ?? throw new InvalidOperationException(
                    $"Категория с Id {dto.CategoryId} не найдена.");

            _mapper.Map(dto, product);

            _repository.Update(product);
            await _repository.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO?> PatсhAsync(
            int id, PatchProductDTO dto, CancellationToken ct)
        {
            var product = await _repository.GetByIdAsync(id, ct);
            if (product is null ) return null;

            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price is not null) product.Price = dto.Price.Value;
            if (dto.Url is not null) product.Url = dto.Url;

            if (dto.CategoryId is not null)
            {
                _ = await _repository.FintCategoryAsync(dto.CategoryId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Категория с Id {dto.CategoryId} не найдена.");
                product.CategoryId = dto.CategoryId.Value;
            }

            _repository.Update(product);
            await _repository.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var product = await _repository.GetByIdAsync(id, ct);
            if ( product is null ) return false;

            _repository.Remove(product);
            await _repository.SaveChangesAsync();
            return true;
        }

    }
}
