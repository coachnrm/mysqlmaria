using System;
using LiveDataMySql.Data;
using LiveDataMySql.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LiveDataMySql.Services;

public class EmployeeService
{
    // private readonly IHubContext<EmployeeHub> _context;
    // private readonly AppDbContext _dbContext;

    // private readonly string _connectionString;
    // public EmployeeService(IHubContext<EmployeeHub> context, AppDbContext dbContext, IConfiguration configuration)
    // {
    //     _context = context;
    //     _dbContext = dbContext;

    //     // Get the connection string from appsettings.json
    //     _connectionString = configuration.GetConnectionString("DefaultConnection");

    // }

    //  public async Task<List<Employee>> GetAllEmployees()
    // {
    //     var employees = await _dbContext.Employee.AsNoTracking().ToListAsync();

    //     await _context.Clients.All.SendAsync("ReceiveMessage");
    //     return employees;
        
    // }
    private readonly string _connectionString = "Server=localhost;Port=3307;Database=CompanyDatabase2;User=root;Password=123456;";
    private readonly Timer _timer;
    public List<Employee> Employees { get; private set; } = new();

    public event Action? OnChange; // Event to notify components of data changes

    public EmployeeService()
    {
        // Set up the timer to fetch data every 10 seconds
        _timer = new Timer(FetchEmployees, null, 0, 10000); // Start immediately, then every 10s
    }

    private void FetchEmployees(object state)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Employee";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    var employeeList = new List<Employee>();

                    while (reader.Read())
                    {
                        employeeList.Add(new Employee
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Title = reader.GetString("title")
                        });
                    }

                    Employees = employeeList;

                    // Notify subscribers (Blazor components) of changes
                    NotifyDataChanged();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching employees: {ex.Message}");
        } 
    }

    private void NotifyDataChanged()
    {
        OnChange?.Invoke(); // Trigger the event
    }
}


