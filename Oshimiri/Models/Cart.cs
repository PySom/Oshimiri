using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Generated.Mapper.Helpers;
using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Models;

[Mapper]
public class Cart : Audit
{
    [Map<CartDTO>]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Map<CartViewModel>]
    [Map<CartDTO>]
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Map<CartViewModel>]
    [Map<CartDTO>]
    [ForeignKey(nameof(OrderId))]
    public int? OrderId { get; set; }
    public Order? Orders { get; set; }

}
