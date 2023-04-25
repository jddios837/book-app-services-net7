// See https://aka.ms/new-console-template for more information
using Packt.Shared; // Product
using System.Net.Http.Json; // ReadFromJsonAsync<T>

Write("Enter a client name: ");
string? clientName = ReadLine();

if (string.IsNullOrEmpty(clientName))
{
    clientName = $"console-client-{Guid.NewGuid()}";
}

WriteLine($"X-Client-Id wil be: {clientName}");

HttpClient client = new();
client.BaseAddress = new Uri("http://localhost:5173");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new("application/json"));

// specify rate limiting client id
client.DefaultRequestHeaders.Add("X-Client-Id", clientName);

ConsoleColor previousColor;

while (true)
{
    previousColor = ForegroundColor;
    ForegroundColor = ConsoleColor.DarkGreen;
    Write("{0:hh:mm:ss} ", DateTime.UtcNow);
    ForegroundColor = previousColor;

    int waitFor = 1; // seconds

    try
    {
        HttpResponseMessage response = await client.GetAsync("api/products");

        if (response.IsSuccessStatusCode)
        {
            Product[]? products = await response.Content.ReadFromJsonAsync<Product[]>();
            
            if(products != null)
            {
                foreach (var p in products)
                {
                    Write(p.ProductName);
                    Write(", ");
                }
                WriteLine();
            }
            else
            {
                previousColor = ForegroundColor;
                ForegroundColor = ConsoleColor.DarkBlue;
                WriteLine($"{(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
                ForegroundColor = previousColor;
            }
        }
    }
    catch (Exception e)
    {
        WriteLine(e.Message);
        throw;
    }
    
    await Task.Delay(TimeSpan.FromSeconds(waitFor));
}