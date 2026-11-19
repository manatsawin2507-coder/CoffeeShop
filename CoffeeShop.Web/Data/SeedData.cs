using CoffeeShop.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Web.Data;

public static class SeedData
{
    public static readonly string[] Roles = { "Admin", "Manager", "Cashier", "Barista" };

    public static async Task InitializeAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<CoffeeShopDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await CreateUserAsync(db, userManager, "admin@coffee.local", "Admin@1234",
            "Admin", "EMP001", "เจ้าของร้าน", "เจ้าของร้าน");
        await CreateUserAsync(db, userManager, "cashier@coffee.local", "Cashier@1234",
            "Cashier", "EMP002", "พนักงานแคชเชียร์", "แคชเชียร์");

        await SeedMochaBonusAsync(db);

        if (await db.Categories.AnyAsync()) return;

        var coffee = new Category { Name = "กาแฟ", SortOrder = 1 };
        var tea = new Category { Name = "ชา", SortOrder = 2 };
        var bakery = new Category { Name = "เบเกอรี่", SortOrder = 3 };

        var size = new OptionGroup
        {
            Name = "ขนาด",
            IsRequired = true,
            MaxSelect = 1,
            Options =
            {
                new Option { Name = "ปกติ 16 oz", ExtraPrice = 0 },
                new Option { Name = "ใหญ่ 22 oz", ExtraPrice = 10 }
            }
        };

        var sweet = new OptionGroup
        {
            Name = "ระดับความหวาน",
            IsRequired = true,
            MaxSelect = 1,
            Options =
            {
                new Option { Name = "หวานน้อย" },
                new Option { Name = "หวานปกติ" },
                new Option { Name = "หวานมาก" }
            }
        };

        var topping = new OptionGroup
        {
            Name = "ท็อปปิ้ง",
            IsRequired = false,
            MaxSelect = 3,
            Options =
            {
                new Option { Name = "เพิ่มช็อต", ExtraPrice = 15 },
                new Option { Name = "วิปครีม", ExtraPrice = 10 }
            }
        };

        var bean = new Ingredient { Name = "เมล็ดกาแฟคั่ว", Unit = "g", CostPerUnit = 0.80m, StockQty = 5000, ReorderLevel = 1000 };
        var milk = new Ingredient { Name = "นมสด", Unit = "ml", CostPerUnit = 0.05m, StockQty = 10000, ReorderLevel = 2000 };
        var syrup = new Ingredient { Name = "น้ำเชื่อม", Unit = "ml", CostPerUnit = 0.03m, StockQty = 3000, ReorderLevel = 500 };
        var teaLeaf = new Ingredient { Name = "ผงชาไทย", Unit = "g", CostPerUnit = 0.60m, StockQty = 2000, ReorderLevel = 500 };
        var cup = new Ingredient { Name = "แก้ว 16 oz", Unit = "ใบ", CostPerUnit = 2.50m, StockQty = 500, ReorderLevel = 100 };

        var espresso = new Product { Name = "เอสเปรสโซเย็น", Category = coffee, BasePrice = 50 };
        var latte = new Product { Name = "ลาเต้เย็น", Category = coffee, BasePrice = 55 };
        var thaiTea = new Product { Name = "ชาไทยเย็น", Category = tea, BasePrice = 45 };
        var croissant = new Product { Name = "ครัวซองต์", Category = bakery, BasePrice = 45 };

        foreach (var drink in new[] { espresso, latte, thaiTea })
        {
            drink.ProductOptionGroups.Add(new ProductOptionGroup { OptionGroup = size });
            drink.ProductOptionGroups.Add(new ProductOptionGroup { OptionGroup = sweet });
            drink.ProductOptionGroups.Add(new ProductOptionGroup { OptionGroup = topping });
        }

        espresso.Recipes.Add(new Recipe { Ingredient = bean, Quantity = 18 });
        espresso.Recipes.Add(new Recipe { Ingredient = syrup, Quantity = 20 });
        espresso.Recipes.Add(new Recipe { Ingredient = cup, Quantity = 1 });

        latte.Recipes.Add(new Recipe { Ingredient = bean, Quantity = 18 });
        latte.Recipes.Add(new Recipe { Ingredient = milk, Quantity = 150 });
        latte.Recipes.Add(new Recipe { Ingredient = cup, Quantity = 1 });

        thaiTea.Recipes.Add(new Recipe { Ingredient = teaLeaf, Quantity = 15 });
        thaiTea.Recipes.Add(new Recipe { Ingredient = milk, Quantity = 100 });
        thaiTea.Recipes.Add(new Recipe { Ingredient = syrup, Quantity = 30 });
        thaiTea.Recipes.Add(new Recipe { Ingredient = cup, Quantity = 1 });

        db.AddRange(espresso, latte, thaiTea, croissant);
        await db.SaveChangesAsync();

        await SeedMochaBonusAsync(db);
    }

    private static async Task SeedMochaBonusAsync(CoffeeShopDbContext db)
    {
        var chocoSauce = await db.Ingredients
            .FirstOrDefaultAsync(i => i.Name == "ซอสช็อกโกแลต");

        if (chocoSauce == null)
        {
            chocoSauce = new Ingredient
            {
                Name = "ซอสช็อกโกแลต",
                Unit = "ml",
                CostPerUnit = 0.08m,
                StockQty = 2000,
                ReorderLevel = 400
            };
            db.Ingredients.Add(chocoSauce);
            await db.SaveChangesAsync();
        }

        var mocha = await db.Products
            .FirstOrDefaultAsync(p => p.Name == "มอคค่าเย็น");

        if (mocha == null)
        {
            var coffeeCat = await db.Categories.FirstOrDefaultAsync(c => c.Name == "กาแฟ");
            var beanIng = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "เมล็ดกาแฟคั่ว");
            var milkIng = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "นมสด");
            var cupIng = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "แก้ว 16 oz");

            var sizeGroup = await db.OptionGroups.FirstOrDefaultAsync(g => g.Name == "ขนาด");
            var sweetGroup = await db.OptionGroups.FirstOrDefaultAsync(g => g.Name == "ระดับความหวาน");
            var toppingGroup = await db.OptionGroups.FirstOrDefaultAsync(g => g.Name == "ท็อปปิ้ง");

            mocha = new Product
            {
                Name = "มอคค่าเย็น",
                CategoryId = coffeeCat?.CategoryId ?? 1,
                BasePrice = 55
            };

            if (sizeGroup != null) mocha.ProductOptionGroups.Add(new ProductOptionGroup { OptionGroupId = sizeGroup.OptionGroupId });
            if (sweetGroup != null) mocha.ProductOptionGroups.Add(new ProductOptionGroup { OptionGroupId = sweetGroup.OptionGroupId });
            if (toppingGroup != null) mocha.ProductOptionGroups.Add(new ProductOptionGroup { OptionGroupId = toppingGroup.OptionGroupId });

            if (beanIng != null) mocha.Recipes.Add(new Recipe { IngredientId = beanIng.IngredientId, Quantity = 18 });
            if (milkIng != null) mocha.Recipes.Add(new Recipe { IngredientId = milkIng.IngredientId, Quantity = 120 });
            mocha.Recipes.Add(new Recipe { IngredientId = chocoSauce.IngredientId, Quantity = 25 });
            if (cupIng != null) mocha.Recipes.Add(new Recipe { IngredientId = cupIng.IngredientId, Quantity = 1 });

            db.Products.Add(mocha);
            await db.SaveChangesAsync();
        }
    }

    private static async Task CreateUserAsync(
        CoffeeShopDbContext db,
        UserManager<IdentityUser> userManager,
        string email,
        string password,
        string role,
        string code,
        string fullName,
        string position)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            await userManager.AddToRoleAsync(user, role);
        }

        if (!await db.Employees.AnyAsync(e => e.UserId == user.Id))
        {
            db.Employees.Add(new Employee
            {
                UserId = user.Id,
                EmployeeCode = code,
                FullName = fullName,
                Position = position,    
                HireDate = DateTime.Today
            });
            await db.SaveChangesAsync();
        }
    }   
}