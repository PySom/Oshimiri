using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Services;

public interface IProductService
{
    Task<ProductDTO> CreateProductAsync(ProductViewModel dto);
    Task<string?> UploadProductImageAsync(IFormFile file);
    Task<ProductDTO[]> GetAllProductsAsync();
    Task<ProductDTO?> GetProductByIdAsync(int id);
}