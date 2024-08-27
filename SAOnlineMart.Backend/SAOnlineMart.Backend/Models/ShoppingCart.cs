using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ShoppingCart
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
}
