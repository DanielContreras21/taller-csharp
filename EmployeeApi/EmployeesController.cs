using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 

[ApiController] 
[Route("[controller]")] 
public class EmployeesController : ControllerBase 
{ 
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context) 
    { 
        _context = context; 
    }

    [HttpPost] 
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    { 
        _context.Employees.Add(employee); 
        await _context.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, 
employee); 
    }

    [HttpGet("{id}")] 
    public async Task<ActionResult<Employee>> GetEmployee(int id) 
    { 
        var employee = await _context.Employees.FindAsync(id); 
        if (employee == null) return NotFound(); 
        return employee; 
    }

    [HttpPut("{id}")] 
    public async Task<IActionResult> PutEmployee(int id, Employee employee) 
    { 
        if (id != employee.Id) return BadRequest(); 
        _context.Entry(employee).State = EntityState.Modified; 
        try 
        { 
            await _context.SaveChangesAsync(); 
        } 
        catch (DbUpdateConcurrencyException) 
        { 
            if (!EmployeeExists(id)) return NotFound(); 
            throw; 
        } 
        return NoContent(); 
    } 

    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteEmployee(int id) 
    { 
        var employee = await _context.Employees.FindAsync(id); 
        if (employee == null) return NotFound(); 
        _context.Employees.Remove(employee); 
        await _context.SaveChangesAsync(); 
        return NoContent(); 
    } 
    private bool EmployeeExists(int id) => _context.Employees.Any(e => e.Id == id); 
}