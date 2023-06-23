using CarRental.Services.Catalog.Dtos;
using CarRental.Shared.Dtos;

namespace CarRental.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<ResponseDto<List<CreatedCategoryDto>>> GetAllAsync();
        Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto categoryDto);
        Task<ResponseDto<CategoryDto>> GetByIdAsync(string id);
    }
}
