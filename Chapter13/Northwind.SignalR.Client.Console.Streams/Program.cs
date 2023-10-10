using Microsoft.AspNetCore.SignalR.Client;
using Northwind.SignalR.Streams;

WriteLine("Enter a Stock (press ENTER for MSFT):");
string? stock = ReadLine();

if (string.IsNullOrWhiteSpace(stock))
{
    stock = "MSFT";
}

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5132/stockprice")
    .Build();
    
await  connection.StartAsync();

try
{
    CancellationTokenSource cts = new();
    
    IAsyncEnumerable<StockPrice> stockPrices = connection.StreamAsync<StockPrice>(
        "GetStockPriceUpdates", stock, cts.Token);

    await foreach (var sp in stockPrices)
    {
        WriteLine($"{sp.Stock} is now {sp.Price:C}.");
        
        Write("Do you want to cancel (y/n)? ");
        ConsoleKey key = ReadKey().Key;
        if (key == ConsoleKey.Y)    
        {
            cts.Cancel();
        }
        
        WriteLine();
    }
}
catch (Exception e)
{
    WriteLine($"{e.GetType()} says {e.Message}");
}
WriteLine();

WriteLine("Streaming download completed.");

await connection.SendAsync("UploadStocks", GetStockAsync());

WriteLine("Uploading stocks ... press ENTER to exit.");
ReadLine();

WriteLine("Ending console app.");