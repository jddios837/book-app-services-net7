// See https://aka.ms/new-console-template for more information
using static System.Console;

HttpClient client = new();
HttpResponseMessage responseMessage = await client.GetAsync("https://www.apple.com/");

WriteLine($"Apple's home page has {0:N0} bytes.", responseMessage.Content.Headers.ContentLength);