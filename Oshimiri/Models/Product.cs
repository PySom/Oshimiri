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
