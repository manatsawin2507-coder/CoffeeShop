SELECT p.Name AS เมนู,
       SUM(r.Quantity * i.CostPerUnit) AS ต้นทุนต่อแก้ว
FROM Recipes r
JOIN Products p ON p.ProductId = r.ProductId
JOIN Ingredients i ON i.IngredientId = r.IngredientId
GROUP BY p.Name;

SELECT u.Email, ro.Name AS Role, e.EmployeeCode, e.FullName
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON ur.UserId = u.Id
JOIN AspNetRoles ro ON ro.Id = ur.RoleId
JOIN Employees e ON e.UserId = u.Id;