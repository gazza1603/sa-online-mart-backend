using System.ComponentModel.DataAnnotations;

public class OrderItem
{
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000")]
    public decimal Price { get; set; }
    public Product Product { get; set; }
}
