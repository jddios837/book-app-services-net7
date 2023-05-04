using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Packt.Shared;

namespace Northwind.Odata.Service.Controllers;

public class CategoriesController : ODataController
{
    private readonly NorthwindContext _db;

    public CategoriesController(NorthwindContext db)
    {
        this._db = db;
    }
    
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_db.Categories);
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        return Ok(_db.Categories.Where(category => category.CategoryId == key));
    }
}