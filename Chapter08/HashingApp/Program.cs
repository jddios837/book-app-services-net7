// See https://aka.ms/new-console-template for more information
using Packt.Shared;

WriteLine("Registering Alice with Pa$$w0rd");
User alice = Protector.Register("Alice", "Pa$$w0rd");

WriteLine($"   Name: {alice.Name}");
WriteLine($"   Salt: {alice.Salt}");
WriteLine($"   Password (salted and hashed): {alice.SaltedHashedPassword}");
WriteLine();

