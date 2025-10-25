using Moq; 
using Xunit; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 

public class EmployeeTests 
{ 
    [Fact] 
    public async Task CreateEmployee() 
    { 
        var options = new DbContextOptionsBuilder<AppDbContext>() 
            .UseInMemoryDatabase(databaseName: "Test_Create") 
            .Options; 
        using var context = new AppDbContext(options); 
        var controller = new EmployeesController(context); 
        var employee = new Employee { Name = "John", Position = "Dev", Salary = 50000 
}; 
        var result = await controller.PostEmployee(employee); 
        Assert.IsType<CreatedAtActionResult>(result.Result); 
        var created = (CreatedAtActionResult)result.Result!; 
        Assert.Equal(1, ((Employee)created.Value!).Id); 
    } 
    [Fact] 
    public async Task GetEmployee() 
    { 
        var options = new DbContextOptionsBuilder<AppDbContext>() 
            .UseInMemoryDatabase(databaseName: "Test_Get") 
            .Options; 
        using var context = new AppDbContext(options); 
        context.Employees.Add(new Employee { Id = 1, Name = "John", Position = "Dev", 
Salary = 50000 }); 
        await context.SaveChangesAsync(); 
        var controller = new EmployeesController(context); 
        var result = await controller.GetEmployee(1); 
        Assert.IsType<ActionResult<Employee>>(result); 
        Assert.Equal("John", result.Value!.Name); 
    } 
}