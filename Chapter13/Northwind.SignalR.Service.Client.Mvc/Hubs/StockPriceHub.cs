using Microsoft.AspNetCore.SignalR;
using Northwind.SignalR.Streams;

namespace Northwind.SignalR.Service.Client.Mvc.Hubs;

public class StockPriceHub : Hub
{
    public async IAsyncEnumerable<StockPrice> GetStockPrices(string stockSymbol, CancellationToken cancellationToken)
    {
        double currentPrice = 267.10; // Simulated initial price.

        for (int i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            currentPrice += (Random.Shared.NextDouble() * 10.0) - 5.0;

            StockPrice stockPrice = new(stockSymbol, currentPrice);
            
            Console.WriteLine("[{0}] {1} at {2:C",
                DateTime.UtcNow, stockPrice.Stock, stockPrice.Price);
            
            yield return stockPrice;

            await Task.Delay(4000, cancellationToken);
        }
    }

    public async Task UploadStocks(IAsyncEnumerable<string> stocks)
    {
        await foreach (string stock in stocks)
        {
            Console.WriteLine($"Uploading {stock}...");
        }
    }
}