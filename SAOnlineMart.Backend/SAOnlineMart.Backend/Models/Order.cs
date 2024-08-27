using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Order
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Order date is required")]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Total amount is required")]
    [Range(0.01, 1000000, ErrorMessage = "Total amount must be between 0.01 and 1,000,000")]
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
