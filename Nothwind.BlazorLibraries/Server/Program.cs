using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc;
using Packt.Shared;
using System.Text.Json.Serialization;
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

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
