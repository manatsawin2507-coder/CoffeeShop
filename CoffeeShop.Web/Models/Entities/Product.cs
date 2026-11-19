using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Web.Models.Entities;

public class Product
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    public Category Category { get; set; } = null!;
    public ICollection<ProductOptionGroup> ProductOptionGroups { get; set; } = new List<ProductOptionGroup>();
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}