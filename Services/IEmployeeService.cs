using System;
using LiveDataMySql.Data;

namespace LiveDataMySql.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllEmployees();
    void StartPolling();
    void StopPolling();
}
