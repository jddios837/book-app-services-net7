using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using Packt.Shared;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext db;

    public HomeController(ILogger<HomeController> logger, NorthwindContext _db)
    {
        _logger = logger;
        this.db = _db;
    }

    public IActionResult Index()
    {
        IEnumerable<Order> model = db.Orders
            .Include(order => order.Customer)
            .Include(order => order.OrderDetails)
            .OrderByDescending(order => order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
            .AsEnumerable();
        
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}