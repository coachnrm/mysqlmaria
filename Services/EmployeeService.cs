using System;
using LiveDataMySql.Data;
using LiveDataMySql.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveDataMySql.Services;

public class EmployeeService
{
    private readonly IHubContext<EmployeeHub> _context;
    private readonly AppDbContext _dbContext;

    private readonly string _connectionString;
    public EmployeeService(IHubContext<EmployeeHub> context, AppDbContext dbContext, IConfiguration configuration)
    {
        _context = context;
        _dbContext = dbContext;

        // Get the connection string from appsettings.json
        _connectionString = configuration.GetConnectionString("DefaultConnection");

    }

     public async Task<List<Employee>> GetAllEmployees()
    {
        var employees = await _dbContext.Employee.AsNoTracking().ToListAsync();

        await _context.Clients.All.SendAsync("ReceiveMessage");
        return employees;
        
    }
}


