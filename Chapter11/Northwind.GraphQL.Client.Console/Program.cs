// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hola");
// using Microsoft.Extensions.DependencyInjection;
// using Northwind.GraphQL.Client.Console;
// using StrawberryShake;
//
// ServiceCollection serviceCollection = new();
// // Exclude the Generated File to avoid errors in Rider
// serviceCollection
//     .AddNorthwindClient()
//     .ConfigureHttpClient(client => 
//         client.BaseAddress = new Uri("http://localhost:512/graphql"));
//
// IServiceProvider services = serviceCollection.BuildServiceProvider();
// INorthwindClient client = services.GetRequiredService<INorthwindClient>();
//
// var result = await client.SeafoodProducts.ExecuteAsync();
// result.EnsureNoErrors();
//
// if (result.Data is null)
// {
//     WriteLine("No data!");
//     return;
// }
//
// foreach (var product in result.Data.ProductsInCategory)
// {
//     WriteLine("{0}: {1}",
//         product.ProductId, product.ProductName);
// }