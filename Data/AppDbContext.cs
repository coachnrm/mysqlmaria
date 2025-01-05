using System;
using LiveDataMySql.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveDataMySql.Data;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employee { get; set; }

    // Constructor that accepts DbContextOptions
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Optional: If you want to add additional configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity mappings here if necessary
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseMySql(
    //         "Server=localhost;Port=3307;Database=CompanyDatabase2;User=root;Password=123456;",
    //         new MySqlServerVersion(new Version(10, 0, 0))); // Use your MySQL version
    // }

}
