using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc;
using Packt.Shared;
using System.Text.Json.Serialization;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HttpJsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// Inject NorthwindContext on Services
builder.Services.AddNorthwindContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Add a minimal API
app.MapGet("api/employees", ([FromServices] NorthwindContext db) => 
    Results.Json(db.Employees))
    .WithName("GetEmployees")
    .Produces<Employee[]>(StatusCodes.Status200OK);

app.MapGet("api/employees/{id:int}", (
    [FromServices] NorthwindContext db,
    [FromRoute] int id) =>
{
    Employee? employee = db.Employees.Find(id);
    return employee == null ? Results.NotFound() : Results.Json(employee);
})
    .WithName("GetEmployeesById")
    .Produces<Employee>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapGet("api/employees/{country}", (
        [FromServices] NorthwindContext db,
        [FromRoute] string Country) =>
    Results.Json(db.Employees.Where(emp => emp.Country == Country)))
    .WithName("GetEmployeesByCountry")
    .Produces<Employee[]>(StatusCodes.Status200OK);

app.MapPost("api/employees", async (
    [FromBody] Employee employee,
    [FromServices] NorthwindContext db) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
    return Results.Created($"api/employees/{employee.EmployeeId}", employee);
})
    .Produces<Employee>(StatusCodes.Status201Created);

// Categories API
app.MapGet("api/categories", ([FromServices] NorthwindContext db) =>
    Results.Json(db.Categories))
    .WithName("GetCategories")
    .Produces<Category[]>(StatusCodes.Status200OK);



app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();