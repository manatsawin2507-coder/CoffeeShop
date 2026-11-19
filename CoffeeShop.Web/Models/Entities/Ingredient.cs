using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Web.Models.Entities;

public class Ingredient
{
    public int IngredientId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Unit { get; set; } = string.Empty;

    public decimal CostPerUnit { get; set; }
    public decimal StockQty { get; set; }
    public decimal ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}