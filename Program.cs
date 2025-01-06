using LiveDataMySql.Components;
using LiveDataMySql.Data;
using LiveDataMySql.Hubs;
using LiveDataMySql.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5178/") });
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseMySql(
//         "Server=localhost;Port=3307;Database=CompanyDatabase2;User=root;Password=123456;",
//         new MySqlServerVersion(new Version(10, 0, 0))));
builder.Services.AddDbContextFactory<AppDbContext>(opt =>
         opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version())));
//builder.Services.AddScoped<EmployeeService>();
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddSignalR();     
builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
    });

var app = builder.Build();

//Adding Response Compression to the project
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();
app.MapControllers();
app.MapStaticAssets();
app.MapHub<EmployeeHub>("/employeehub");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//Creating an endpoint for the EmployeeHub

app.Run();
