using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs;

public class UpdateProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
