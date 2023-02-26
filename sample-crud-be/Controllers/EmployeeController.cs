using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample_crud_be.Models;

namespace sample_crud_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext employeeContext;

        public EmployeeController(EmployeeContext employeeContext) 
        {
            employeeContext = employeeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if(employeeContext.Employees == null)
            {
                return NotFound();
            }
            return await employeeContext.Employees.ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee(int id)
        {
            if (employeeContext.Employees == null)
            {
                return NotFound();
            }
            var employee = employeeContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            employeeContext.Employees.Add(employee);
            await employeeContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new {id=employee.Id},employee);
        }

        [HttpPut("id")]
        public async Task<ActionResult> PutEmployee(int id,Employee employee)
        {
            if(id==employee.Id)
            {
                return BadRequest();
            }
            employeeContext.Entry(employee).State=EntityState.Modified;
            try
            {
                await employeeContext.SaveChanges();
            }

            catch (DbUpdateConcurrencyExceptions)
            {
                throw;
            }

            return Ok();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (employeeContext.Employees == null)
            {
                return NotFound();
            }
            var employee=await employeeContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            employeeContext.Employees.Remove(employee);
            await employeeContext.SaveChangesAsync();   
            return Ok();
        }

    }
}
