using Microsoft.AspNetCore.Mvc;
using Packt.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapGet("api/categories", (
    [FromServices] NorthwindContext db) => db.Categories)
    .WithName("GetCategories")
    .Produces<Category[]>();

app.MapGet("api/categories/{id:int}", (
    [FromRoute] int id,
    [FromServices] NorthwindContext db) => 
    db.Categories.Where(category => category.CategoryId == id))
    .WithName("GetCategory")
    .Produces<Category[]>();

app.MapPost("api/categories", async (
    [FromBody] Category category,
    [FromServices] NorthwindContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"api/categories/{category.CategoryId}", category);
}).Produces<Category>(StatusCodes.Status201Created);

app.MapPut("api/categories/{id:int}", async (
    [FromRoute] int id,
    [FromBody] Category category,
    [FromServices] NorthwindContext db) =>
{
    Category? foundCategory = await db.Categories.FindAsync(id);
    
    if (foundCategory is null) return Results.NotFound();
    
    foundCategory.CategoryName = category.CategoryName;
    foundCategory.Description = category.Description;
    foundCategory.Picture = category.Picture;
    
    await db.SaveChangesAsync();
    
    return Results.NoContent();
}).Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent);

app.MapDelete("api/categories/{id:int", async (
    [FromRoute] int id,
    [FromServices] NorthwindContext db) =>
{
    if(await db.Categories.FindAsync(id) is Category category)
    {
        db.Categories.Remove(category);
        await db.SaveChangesAsync();
        return Results.NotFound();
    }

    return Results.NotFound();
}).Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent);

app.Run();