using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Packt.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNorthwindContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

int pageSize = 10;

app.MapGet("api/products", ([FromServices] NorthwindContext db, [FromQuery] int? page) =>
        db.Products.Where(product =>
                (product.UnitsInStock > 0) && (!product.Discontinued))
            .Skip(((page ?? 1) - 1) * pageSize).Take(pageSize)
    )
    .WithName("GetProducts")
    .WithOpenApi(operation =>
    {
        operation.Description = "Gets products with UnitsInStock > 0 and not Discontinued";
        operation.Summary = "Gets products in stock that are not discontinued";
        return operation;
    })
    .Produces<Product[]>(StatusCodes.Status200OK);

app.Run();