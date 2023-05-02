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

    public IActionResult Get()
    {
        return Ok(_db.Products);
    }

    public IActionResult Get(int key)
    {
        return Ok(_db.Products.Where(product => product.ProductId == key));
    }
}