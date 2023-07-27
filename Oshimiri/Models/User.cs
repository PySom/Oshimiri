using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Generated.Mapper.Helpers;
using Oshimiri.DataTransfer;
using Oshimiri.ViewModels;

namespace Oshimiri.Models;

[Mapper]
public class User : Audit
{
    [Map<UserDTO>]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Map<UserDTO>]
    [Map<UserViewModel>]
    [StringLength(30)]
    public required string Email { get; set; }


    [Map<UserDTO>]
    [Map<UserViewModel>]
    [StringLength(20)]
    public required string FirstName { get; set; }


    [Map<UserDTO>]
    [Map<UserViewModel>]
    [StringLength(20)]
    public required string LastName { get; set; }

    [Map<UserDTO>]
    [Map<UserViewModel>]
    [StringLength(80)]
    public string? Address { get; set; }
}
