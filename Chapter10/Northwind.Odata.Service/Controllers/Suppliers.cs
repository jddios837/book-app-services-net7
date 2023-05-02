using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Packt.Shared;

namespace Northwind.Odata.Service.Controllers;

public class Suppliers : ODataController
{
    private readonly NorthwindContext _db;
    
    public Suppliers(NorthwindContext db)
    {
        _db = db;
    }

    public IActionResult Get()
    {
        return Ok(_db.Suppliers);
    }

    public IActionResult Get(int key)
    {
        return Ok(_db.Suppliers.Where(supplier => supplier.SupplierId == key));
    }
}