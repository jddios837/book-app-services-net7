using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Grpc.Client.Mvc.Models;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;


namespace Northwind.Grpc.Client.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    protected readonly Greeter.GreeterClient _greeterClient;

    public HomeController(ILogger<HomeController> logger,
        GrpcClientFactory factory)
    {
        _logger = logger;
        _greeterClient = factory.CreateClient<Greeter.GreeterClient>("Greeter");
    }

    public async Task<IActionResult> Index(string name = "Henrietta")
    {
        try
        {
            var reply = await _greeterClient.SayHelloAsync(new HelloRequest { Name = name });
            ViewData["greeting"] = "Greeting from gRPC service: " + reply.Message;
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Northwind.Grpc.Service is not responding.");
            ViewData["exception"] = e.Message;
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