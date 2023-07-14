using Oshimiri.Services;
using Oshimiri.ViewModels;

namespace Oshimiri.Endpoints;

public static class ProductsEndpoint
{
    public static IEndpointConventionBuilder MapProducts(this IEndpointRouteBuilder endpoints)
    {
        var productGroup = endpoints
            .MapGroup("products")
            .WithTags("Products")
            .WithOpenApi();

        productGroup.MapGet("", async (IProductService productService) =>
        {
            var products = await productService.GetAllProductsAsync();
            return TypedResults.Ok(products);
        }).WithName("GetAllProducts");

        productGroup.MapGet("{id:int}", async (int id, IProductService productService) =>
        {
            var product = await productService.GetProductByIdAsync(id);
            return product switch
            {
                not null => Results.Ok(product),
                _ => Results.NotFound()
            };
        }).WithName("GetProductById");

        productGroup.MapPost("", async (ProductViewModel dto,
            IProductService productService) =>
        {
            var product = await productService.CreateProductAsync(dto);
            return TypedResults.Created($"{product.Id}", product);
        }).WithName("CreateProduct");

        productGroup.MapPost("upload", async (IFormFile file,
            IProductService productService) =>
        {
            return TypedResults.Ok(await productService.UploadProductImageAsync(file));
        }).WithName("UploadProductImage");

        return productGroup;
    }
}
