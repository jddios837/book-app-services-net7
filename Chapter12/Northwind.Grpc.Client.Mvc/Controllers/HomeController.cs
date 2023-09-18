using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Grpc.Client.Mvc.Models;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Grpc.Core;


namespace Northwind.Grpc.Client.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    protected readonly Greeter.GreeterClient _greeterClient;
    protected readonly Shipper.ShipperClient _shipperClient;

    public HomeController(ILogger<HomeController> logger,
        GrpcClientFactory factory)
    {
        _logger = logger;
        _greeterClient = factory.CreateClient<Greeter.GreeterClient>("Greeter");
        _shipperClient = factory.CreateClient<Shipper.ShipperClient>("Shipper");
    }

    public async Task<IActionResult> Index(string name = "Henrietta", int id = 1)
    {
        try
        {
            var reply = await _greeterClient.SayHelloAsync(new HelloRequest { Name = name });
            ViewData["greeting"] = "Greeting from gRPC service: " + reply.Message;

            ShipperReply shipperReply = await _shipperClient.GetShipperAsync(
                new ShipperRequest { ShipperId = id },
                deadline: DateTime.UtcNow.AddSeconds(3));

            // AsyncUnaryCall<ShipperReply> shipperCall =
            //     _shipperClient.GetShipperAsync(new ShipperRequest { ShipperId = id });
            //
            // Metadata metadata = await shipperCall.ResponseHeadersAsync;
            //
            // foreach (var entry in metadata)
            // {
            //     _logger.LogCritical($"Key: {entry.Key}, Value: {entry.Value}");
            // }

            // ShipperReply shipperReply = await shipperCall.ResponseAsync;

            ViewData["shipper"] = "Shipper from gRPC service: " +
                                  $"ID: {shipperReply.ShipperId}, Name: {shipperReply.CompanyName}," +
                                  $" Phone: {shipperReply.Phone}.";
        }
        catch (RpcException rpcex) when (rpcex.StatusCode == global::Grpc.Core.StatusCode.DeadlineExceeded)
        {
            _logger.LogWarning("Northwind.Grpc.Service deadline exceeded.");
            ViewData["exception"] = rpcex.Message;
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