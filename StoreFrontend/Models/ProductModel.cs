using System.ComponentModel.DataAnnotations;

namespace StoreFrontend.Models;

public class ProductModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}
