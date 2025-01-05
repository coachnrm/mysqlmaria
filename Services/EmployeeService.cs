using System;
using LiveDataMySql.Data;
using LiveDataMySql.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveDataMySql.Services;

// public class EmployeeService
// {
//     private readonly IHubContext<EmployeeHub> _context;
//     private readonly AppDbContext dbContext = new AppDbContext();
//     private readonly Timer _pollingTimer;

//     public EmployeeService(IHubContext<EmployeeHub> context)
//     {
//         _context = context;
//         _pollingTimer = new Timer(PollDatabase, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Poll every 10 seconds
//     }

//     private async void PollDatabase(object state)
//     {
//         var employees = await GetAllEmployees();
//         // Compare with previous state if necessary, and send updates
//         await _context.Clients.All.SendAsync("RefreshEmployees", employees);
//     }

//     public async Task<List<Employee>> GetAllEmployees()
//     {
//         return await dbContext.Employee.AsNoTracking().ToListAsync();
//     }
// }

// public class EmployeeService
// {
//     private readonly IHubContext<EmployeeHub> _context;
//     private readonly AppDbContext _dbContext;
//     private readonly Timer _pollingTimer;

//     public EmployeeService(IHubContext<EmployeeHub> context, AppDbContext dbContext)
//     {
//         _context = context;
//         _dbContext = dbContext;
//         _pollingTimer = new Timer(PollDatabase, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Poll every 10 seconds
//     }

//     private async void PollDatabase(object state)
//     {
//         var employees = await GetAllEmployees();
//         await _context.Clients.All.SendAsync("RefreshEmployees", employees);
//     }

//     public async Task<List<Employee>> GetAllEmployees()
//     {
//         return await _dbContext.Employee.AsNoTracking().ToListAsync();
//     }
// }

public class EmployeeService : IEmployeeService
{
    private readonly IHubContext<EmployeeHub> _context;
    private readonly AppDbContext _dbContext;
    private Timer _pollingTimer;

    public EmployeeService(IHubContext<EmployeeHub> context, AppDbContext dbContext)
    {
        _context = context;
        _dbContext = dbContext;
    }

    private async void PollDatabase(object state)
    {
        var employees = await GetAllEmployees();
        await _context.Clients.All.SendAsync("RefreshEmployees", employees);
        //await _context.Clients.All.SendAsync("ReceiveMessage");
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var employees = await _dbContext.Employee.AsNoTracking().ToListAsync();

        await _context.Clients.All.SendAsync("ReceiveMessage");
        return employees;
        
    }

    public void StartPolling()
    {
        if (_pollingTimer == null)
        {
            _pollingTimer = new Timer(PollDatabase, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)); // Poll every 10 seconds
        }
    }

    public void StopPolling()
    {
        _pollingTimer?.Dispose();
        _pollingTimer = null;
    }

    

}
