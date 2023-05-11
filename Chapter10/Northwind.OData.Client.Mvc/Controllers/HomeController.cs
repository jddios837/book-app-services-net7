using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.OData.Client.Mvc.Models;

namespace Northwind.OData.Client.Mvc.Controllers;

public class HomeController : Controller
{
    protected readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> Index(string startsWith = "Cha")
    {
        try
        {
            HttpClient client = _clientFactory.CreateClient("Northwind.OData");
            string requestUri =
                $"catalog/products/?$filter=startswith(ProductName,'{startsWith}')&$select=ProductId,ProductName,UnitPrice";
            HttpRequestMessage request = new(HttpMethod.Get, requestUri);
            
            HttpResponseMessage response = await client.SendAsync(request);
            
            ViewData["startsWith"] = startsWith;
            ViewData["products"] = (await response.Content.ReadFromJsonAsync<ODataProducts>())?.Value;
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Northwind.OData service exception: {e.Message}");
        }

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
}