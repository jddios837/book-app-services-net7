using Microsoft.AspNetCore.Mvc;
using Packt.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNorthwindContext();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapGet("api/customers", (
    [FromServices] NorthwindContext db) => db.Customers)
    .WithName("GetCustomers")
    .Produces<Customer>();

app.MapGet("api/customers/{id}", (
        [FromRoute] string id,
        [FromServices] NorthwindContext db) => db.Customers
        .FirstOrDefault(c => c.CustomerId == id))
    .WithName("GetCustomer")
    .Produces<Customer>();

app.MapPost("api/customers", async (
    [FromBody] Customer customer,
    [FromServices] NorthwindContext db) =>
{
    db.Customers.Add(customer);
    await db.SaveChangesAsync();
    return Results.Created($"api/customers/{customer.CustomerId}", customer);
}).Produces<Customer>();

app.MapPut("api/customers/{id}", async (
    [FromRoute] string id,
    [FromBody] Customer customer,
    [FromServices] NorthwindContext db) =>
{
    Customer? foundCustomer = await db.Customers.FindAsync(id);
    
    if (foundCustomer is null) return Results.NotFound();
    
    foundCustomer.CompanyName = customer.CompanyName;
    foundCustomer.ContactName = customer.ContactName;
    foundCustomer.ContactTitle = customer.ContactTitle;
    foundCustomer.Address = customer.Address;
    foundCustomer.City = customer.City;
    foundCustomer.Region = customer.Region;
    foundCustomer.PostalCode = customer.PostalCode;
    foundCustomer.Country = customer.Country;
    foundCustomer.Phone = customer.Phone;
    foundCustomer.Fax = customer.Fax;
    
    await db.SaveChangesAsync();
    
    return Results.NoContent();
})
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status204NoContent);

app.MapDelete("api/customers/{id}", async (
    [FromRoute] string id,
    [FromServices] NorthwindContext db) =>
{
    if (await db.Customers.FindAsync(id) is Customer customer)
    {
        db.Customers.Remove(customer);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
}).Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent);

app.Run();