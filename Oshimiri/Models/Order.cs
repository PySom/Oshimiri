using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Generated.Mapper.Helpers;
using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Models;

[Mapper]
public class Order : Audit
{
    [Map<OrderDTO>]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Map<OrderDTO>]
    [Map<OrderViewModel>]
    [ForeignKey(nameof(UserId))]
    public required int UserId { get; set; }
    public User? User { get; set; }
    public List<Cart>? Carts { get; set; }
}
