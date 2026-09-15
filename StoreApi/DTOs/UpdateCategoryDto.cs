using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs;

public class UpdateCategoryDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
