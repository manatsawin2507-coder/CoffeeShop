namespace CoffeeShop.Web.Models.Entities;

public class ProductOptionGroup
{
    public int ProductId { get; set; }
    public int OptionGroupId { get; set; }

    public Product Product { get; set; } = null!;
    public OptionGroup OptionGroup { get; set; } = null!;
}