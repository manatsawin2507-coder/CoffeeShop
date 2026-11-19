using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Web.Models.Entities;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required, StringLength(10)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Position { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;

    public IdentityUser User { get; set; } = null!;
}