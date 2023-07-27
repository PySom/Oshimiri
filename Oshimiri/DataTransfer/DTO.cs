namespace Oshimiri.DataTransfer;

public partial class ProductDTO {}

public partial class CategoryDTO 
{
    public List<ProductDTO>? Products { get; set; }
}
public partial class UserDTO {}
public partial class CartDTO { }
public partial class OrderDTO
{
    public List<CartDTO>? Carts { get; set; }
}