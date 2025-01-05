using System;
using LiveDataMySql.Data;
using Microsoft.AspNetCore.SignalR;

namespace LiveDataMySql.Hubs;

public class EmployeeHub : Hub
{
    public async Task SendMessage()
    {
        await Clients.All.SendAsync("ReceiveMessage");
    }
    public async Task RefreshEmployees(List<Employee> employees)
    {

        await Clients.All.SendAsync("RefreshEmployees", employees);
    }
}
