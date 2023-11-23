using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc;
using Packt.Shared;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using HttpJsonOption = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

// avoid circular references when serializing objects
builder.Services.Configure<HttpJsonOption>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
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

// define endpoints for the Minimal API
app.MapGet("api/categories", ([FromServices] NorthwindContext db) =>
    Results.Json(
        db.Categories.Include(c => c.Products)))
    .WithName("GetCategories")
    .Produces<Category[]>(StatusCodes.Status200OK);

app.MapGet("api/orders/", (NorthwindContext db) => 
    Results.Json(
        db.Orders.Include(o => o.OrderDetails)))
    .WithName("GetOrders")
    .Produces<Order[]>(); // avoid the default 200 OK

app.MapGet("api/employees/", (
    [FromServices] NorthwindContext db) =>
    Results.Json(db.Employees))
    .WithName("GetEmployees")
    .Produces<Employee[]>();

app.MapGet("api/countries/", (
    [FromServices] NorthwindContext db) =>
    Results.Json(db.Employees.Select(emp => emp.Country).Distinct()))
    .WithName("GetCountries")
    .Produces<string[]>();

app.MapGet("app/cities/", (
            [FromServices] NorthwindContext db) =>
        Results.Json(db.Employees.Select(emp => emp.City).Distinct()))
    .WithName("GetCities")
    .Produces<string>();

app.MapPut("api/employees/{id:int}", async (
    [FromRoute] int id,
    [FromBody] Employee employee,
    [FromServices] NorthwindContext db) =>
    {
        var foundEmployee = await db.Employees.FindAsync(id);

        if (foundEmployee is null) return Results.NotFound();
        
        foundEmployee.FirstName = employee.FirstName;
        foundEmployee.LastName = employee.LastName;
        foundEmployee.Title = employee.Title;
        foundEmployee.TitleOfCourtesy = employee.TitleOfCourtesy;
        foundEmployee.BirthDate = employee.BirthDate;
        foundEmployee.HireDate = employee.HireDate;
        foundEmployee.Address = employee.Address;
        foundEmployee.City = employee.City;
        foundEmployee.Region = employee.Region;
        foundEmployee.PostalCode = employee.PostalCode;
        foundEmployee.Country = employee.Country;

        var affected = await db.SaveChangesAsync();

        return Results.Json(affected);
    })
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
