using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.HttpLogging;
using Packt.Shared;
using AspNetCoreRateLimit;

var usingRateLimiting = false;
var builder = WebApplication.CreateBuilder(args);
string northwindMvc = "Northwind.Mvc.Policy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: northwindMvc, policy =>
    {
        policy.WithOrigins("https://localhost:7233");
        policy.WithOrigins("http://localhost:5231/");
    });
});

// Configure the rate limiting middleware.
if (!usingRateLimiting)
{
    // Add services to store rate limit counters and rules
    builder.Services.AddMemoryCache();
    builder.Services.AddInMemoryRateLimiting();
    
    // load default rate limit options from appsettings.json
    builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
    
    // load client-specific policies from appsettings.json
    builder.Services.Configure<ClientRateLimitPolicies>(
        builder.Configuration.GetSection("ClientRateLimitPolicies"));
    
    // register the configuration
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpLogging(options =>
{
    // Add the Origin header so it will not be redacted.
    options.RequestHeaders.Add("Origin");
    
    // Add the rate limit headers so they will not be redacted.
    options.RequestHeaders.Add("X-Client-Id");
    options.ResponseHeaders.Add("Retry-After");
    
    // By default, the response body is not included.
    options.LoggingFields = HttpLoggingFields.All;
});
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

if (!usingRateLimiting)
{
    using var scope = app.Services.CreateScope();
    var clientPolicyStore = scope.ServiceProvider.GetRequiredService<IClientPolicyStore>();
        
    await clientPolicyStore.SeedAsync();
}

app.UseHttpsRedirection();
app.UseHttpLogging();
// app.UseCors(policyName: northwindMvc);
app.UseCors();

app.MapGet("/", () => "Hello World!")
    .ExcludeFromDescription();

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
    .Produces<Product[]>(StatusCodes.Status200OK)
    .RequireCors(policyName: northwindMvc);

app.MapGet("api/products/outofstock", ([FromServices] NorthwindContext db) => 
    db.Products.Where(product => (product.UnitsInStock == 0) && (!product.Discontinued)))
    .WithName("GetProductsOutOfStock")
    .WithOpenApi()
    .Produces<Product[]>(StatusCodes.Status200OK);

app.MapGet("api/products/discontinued", ([FromServices] NorthwindContext db) => 
        db.Products.Where(product => product.Discontinued))
    .WithName("GetProductsDiscontinued")
    .WithOpenApi()
    .Produces<Product[]>(StatusCodes.Status200OK);

app.MapGet("api/products/{id:int}", async Task<Results<Ok<Product>, NotFound>> (
    [FromServices] NorthwindContext db, [FromRoute] int id) => await db.Products.FindAsync(id) is Product product ?
        TypedResults.Ok(product) : TypedResults.NotFound())
    .WithName("GetProductById")
    .WithOpenApi()
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapGet("api/products/{name}", (
        [FromServices] NorthwindContext db, [FromRoute] string name) =>
            db.Products.Where(p => p.ProductName.Contains(name)))
    .WithName("GetProductsByName")
    .WithOpenApi()
    .Produces<Product[]>(StatusCodes.Status200OK)
    .RequireCors(policyName: northwindMvc);

app.MapPost("api/products", async ([FromBody] Product product, [FromServices] NorthwindContext db) =>
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return Results.Created($"api/products/{product.ProductId}", product);
    })
    .WithOpenApi()
    .Produces<Product>(StatusCodes.Status201Created);

app.MapPut("api/products/{id:int}",
    async ([FromRoute] int id, [FromBody] Product product, [FromServices] NorthwindContext db) =>
    {
        Product? foundProduct = await db.Products.FindAsync(id);

        if (foundProduct is null) return Results.NotFound();
        
        foundProduct.ProductName = product.ProductName;
        foundProduct.SupplierId = product.SupplierId;
        foundProduct.CategoryId = product.CategoryId;
        foundProduct.QuantityPerUnit = product.QuantityPerUnit;
        foundProduct.UnitPrice = product.UnitPrice;
        foundProduct.UnitsInStock = product.UnitsInStock;
        foundProduct.UnitsOnOrder = product.UnitsOnOrder;
        foundProduct.ReorderLevel = product.ReorderLevel;
        foundProduct.Discontinued = product.Discontinued;

        await db.SaveChangesAsync();
        
        return Results.NoContent();
    }).WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.MapDelete("api/products/{id:int}", async (
        [FromRoute] int id, 
        [FromServices] NorthwindContext db) =>
    {
        Product? foundProduct = await db.Products.FindAsync(id);

        if (foundProduct is null) return Results.NotFound();

        db.Products.Remove(foundProduct);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }).WithOpenApi()
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();