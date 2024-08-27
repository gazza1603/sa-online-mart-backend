using System.ComponentModel.DataAnnotations;
public class ShoppingCartItem
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public Product Product { get; set; }
}
