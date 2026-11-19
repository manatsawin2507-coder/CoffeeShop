using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Web.Models.Entities;

public class OptionGroup
{
    public int OptionGroupId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsRequired { get; set; }        // ต้องเลือกหรือไม่
    public int MaxSelect { get; set; } = 1;      // เลือกได้สูงสุดกี่ตัวเลือก

    public ICollection<Option> Options { get; set; } = new List<Option>();
    public ICollection<ProductOptionGroup> ProductOptionGroups { get; set; } = new List<ProductOptionGroup>();
}