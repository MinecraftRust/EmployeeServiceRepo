using EmployeeService.Data;
using EmployeeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController (ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/Employee
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Employees);
        }

        // GET: /api/Employee{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (employees == null)
                return Problem(detail: "Employee with id " + id + " is not found.", statusCode: 404);

            return Ok(employees);
        }

        // GET: /api/Employee{gender}
        [HttpGet("{gender}")]
        public IActionResult GetByGender(string? gender = "All")
        {
            switch (gender.ToLower())
            {
                case "all":
                    return Ok(_context.Employees);
                case "male":
                    return Ok(_context.Employees.Where(e => e.Gender.ToLower() == "male"));
                case "female":
                    return Ok(_context.Employees.Where(e => e.Gender.ToLower() == "female"));
                default:
                    return Problem(detail: "Employee with gender " + gender + " is not found.", statusCode: 404);
            }
        }

        // POST: /api/Employee
        [HttpPost]
        public IActionResult Post(Employee employee)
        { 
            _context.Employees.Add(employee);
            _context.SaveChanges();

            return CreatedAtAction("GetAll", new { id = employee.Id }, employee);
        }

        // PUT: /api/Employee
        [HttpPut]
        public IActionResult Put(int? id, Employee employee)
        {
            var entity = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (entity == null)
                return Problem(detail: "Employee with id " + id + " is not found.", statusCode: 404);

            entity.FirtsName = employee.FirtsName;
            entity.LastName = employee.LastName;
            entity.Gender = employee.Gender;
            entity.Salary = employee.Salary;

            _context.SaveChanges();

            return Ok(entity);
        }

        // DELETE: /api/Employee
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var entity = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (entity == null)
                return Problem(detail: "Employee with id " + id + " is not found.", statusCode: 404);

            _context.Employees.Remove(entity);
            _context.SaveChanges();

            return Ok(entity);
        }

    }
}
