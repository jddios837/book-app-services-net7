// See https://aka.ms/new-console-template for more information
using System.Reflection; // Assembly

WriteLine("Assembly metadata:");
Assembly? assembly = Assembly.GetEntryAssembly();

if (assembly is null)
{
    WriteLine("Failed to get entry assembly.");
    return;
}

WriteLine($" Full name: {assembly.FullName}");
WriteLine($" Location : {assembly.Location}");
WriteLine($" Entry point: {assembly.EntryPoint?.Name}");

IEnumerable<Attribute> attributes = assembly.GetCustomAttributes();
WriteLine($" Assembly-level attributes:");
foreach (Attribute attribute in attributes)
{
    WriteLine($"         {attribute.GetType()}");
}