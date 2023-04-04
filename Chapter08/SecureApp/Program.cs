// See https://aka.ms/new-console-template for more information
using Packt.Shared;
using System.Security.Principal;
using System.Security.Claims;

Protector.Register("Alice", "Pa$$w0rd", roles: new string[] { "Admins" });
Protector.Register("Bob", "Pa$$w0rd", roles: new string[] { "Sales", "TeamLeads" });

Protector.Register("Eva", "Pa$$w0rd");

Console.WriteLine("Hello, World!");