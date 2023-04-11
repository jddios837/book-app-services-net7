using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.WebApi.Client.Mvc.Models;
using Packt.Shared;

namespace Northwind.WebApi.Client.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _clientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
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

    public async Task<IActionResult> Products(string? name)
    {
        HttpClient client = _clientFactory.CreateClient("Northwind.WebApi.Service");
        HttpRequestMessage request = new(HttpMethod.Get, $"api/products/{name}");
        
        HttpResponseMessage response = await client.SendAsync(request);

        IEnumerable<Product>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        
        ViewData["baseaddress"] = client.BaseAddress;

        return View(model);
    }
}