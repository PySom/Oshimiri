using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Generated.Mapper.Helpers;
using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Models;

[Mapper]
public class Product : Audit
{
    [Map<ProductDTO>]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Map<ProductDTO>]
    [Map<ProductViewModel>]
    [StringLength(30)]
    public required string Name { get; set; }

    [Map<ProductDTO>]
    [Map<ProductViewModel>]
    [StringLength(70)]
    public string? Description { get; set; }

    [Map<ProductDTO>]
    [Map<ProductViewModel>]
    [StringLength(70)]
    public string? ImageUrl { get; set; }

    [Map<ProductDTO>]
    [Map<ProductViewModel>]
    public int TotalAvailable { get; set; }

    [Map<ProductDTO>]
    [Map<ProductViewModel>]
    public decimal Amount { get; set; }

    [Map<ProductDTO>]
    [Map<ProductViewModel>]
    [ForeignKey(nameof(CategoryId))]
    public int CategoryId { get; set; }

}


[Mapper]
public class Category : Audit
{
    [Map<CategoryDTO>]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [Map<CategoryDTO>]
    [Map<CategoryViewModel>]
    [StringLength(20)]
    public required string Name { get; set; }

    [StringLength(50)]
    [Map<CategoryDTO>]
    [Map<CategoryViewModel>]
    public string? Description { get; set; }
    public List<Product> Products { get; set; } = new();
}

[Mapper]
public class User : Audit
{
    [Map<UserDTO>]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Map<UserDTO>]
    [StringLength(30)]
    public required string Email { get; set; }


    [Map<UserDTO>]
    [StringLength(20)]
    public required string FirstName { get; set; }


    [Map<UserDTO>]
    [StringLength(20)]
    public required string LastName { get; set; }
    [StringLength(80)]
    public string? Address { get; set; }
}

public class Cart : Audit
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public required Product Product { get; set; }
    [ForeignKey(nameof(OrderId))]
    public int? OrderId { get; set; }
    public Order? Orders { get; set; }

}
public class Order : Audit
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey(nameof(UserId))]
    public required int UserId { get; set; }
    public required User User { get; set; }
    public required List<Cart> Carts { get; set; }
}
