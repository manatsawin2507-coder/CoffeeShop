using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Web.Models.Entities;

public class Option
{
    public int OptionId { get; set; }
    public int OptionGroupId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public decimal ExtraPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public OptionGroup OptionGroup { get; set; } = null!;
}