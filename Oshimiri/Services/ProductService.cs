using Microsoft.EntityFrameworkCore;
using Oshimiri.Data;
using Oshimiri.DataTransfer;
using Oshimiri.Exceptions;
using Oshimiri.Models;
using Oshimiri.ViewModels;
using Roselyn.Generated.Source.Extension;

namespace Oshimiri.Services;

public sealed class ProductService : BaseModelService<Product>, IProductService
{
    private readonly IFileService fileService;
    private readonly OshimiriDbContext context;
    private readonly ILogger<ProductService> logger;

    public ProductService(IFileService fileService,
        OshimiriDbContext context,
        ILogger<ProductService> logger)
    {
        this.fileService = fileService;
        this.context = context;
        this.logger = logger;
    }

    public Task<ProductDTO?> GetProductByIdAsync(int id)
    {
        return context.Products.Where(product => product.Id == id)
            .Select(static product => product.ToProductDTO()).FirstOrDefaultAsync();
    }

    public Task<ProductDTO[]> GetAllProductsAsync()
    {
        return context.Products.Select(static product => product.ToProductDTO()).ToArrayAsync();
    }

    public async Task<ProductDTO> CreateProductAsync(ProductViewModel dto)
    {
        try
        {
            Product product = UpdateAudit(dto.ToProduct());
            await context.AddAsync(product);
            await context.SaveChangesAsync();
            return product.ToProductDTO();

        }
        catch (OperationCanceledException ex)
        {
            fileService.RemoveImageAsync(dto.ImageUrl);
            logger.LogError("{Error} occured", ex.Message);
            throw new CreateProductException(ex.Message, ex);
        }
    }

    public async Task<string?> UploadProductImageAsync(IFormFile file)
    {
        await fileService.TryUploadFileAsync(file, out string? fileName);
        return fileName;
    }


}
