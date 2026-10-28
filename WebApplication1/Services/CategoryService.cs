using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<CategoryDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var categories = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<CategoryDTO>>(categories), total);
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var category = await _repository.GetByIdAsync(id, ct);
            return category is null ? null : _mapper.Map<CategoryDTO?>(category);
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto, CancellationToken ct)
        {
            var category = _mapper.Map<Category>(dto);

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(category.IdCategori, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданную категорию.");
            return _mapper.Map<CategoryDTO>(saved);
        }

        public async Task<CategoryDTO?> UpdateAsync(
            int id, UpdateCategoryDTO dto, CancellationToken ct)
        {
            var category = await _repository.GetByIdAsync(id, ct);
            if (category is null) return null;

            _mapper.Map(dto, category);

            _repository.Update(category);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO?> PatсhAsync(
            int id, PatchCategoryDTO dto, CancellationToken ct)
        {
            var category = await _repository.GetByIdAsync(id, ct);
            if (category is null) return null;

            if (dto.Name is not null) category.Name = dto.Name;

            _repository.Update(category);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var category = await _repository.GetByIdAsync(id, ct);
            if (category is null) return false;

            _repository.Remove(category);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
