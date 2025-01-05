using System;
using LiveDataMySql.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveDataMySql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context; 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
    {
        return await _context.Employee.ToListAsync();
    }
}
