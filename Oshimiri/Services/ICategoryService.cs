using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Services;

public interface ICategoryService
{
    Task<CategoryDTO> CreateCategoryAsync(CategoryViewModel dto);
    Task<CategoryDTO[]> GetAllCategoriesAsync();
    Task<CategoryDTO?> GetCategoryByIdAsync(int id);
}