using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Generated.Mapper.Helpers;
using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Models;

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
    public List<Product>? Products { get; set; }
}
