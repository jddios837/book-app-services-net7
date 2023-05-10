using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Packt.Shared;

namespace Northwind.Odata.Service.Controllers;

public class ProductsController : ODataController
{
    private readonly NorthwindContext _db;

    public ProductsController(NorthwindContext db)
    {
        _db = db;
    }

    [EnableQuery]
    public IActionResult Get(string version = "1")
    {
        Console.WriteLine($"*** ProductsController version {version}");
        return Ok(_db.Products);
    }

    [EnableQuery]
    public IActionResult Get(int key, string version = "1")
    {
        Console.WriteLine($"*** ProductsController version {version}");

        IQueryable<Product> products = _db.Products.Where(product => product.ProductId == key);

        Product? p = products.FirstOrDefault();

        if ((products is null) || (p is null))
        {
            return NotFound($"Product with id {key} not found");
        }

        if (version == "2")
        {
            p.ProductName += " version 2.0";
        }

        return Ok(p);
    }

    public IActionResult Post([FromBody] Product product)
    {
        _db.Products.Add(product);
        _db.SaveChanges();
        return Created(product);
    }
}