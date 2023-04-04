// See https://aka.ms/new-console-template for more information

using System.Security;
using Packt.Shared;
using System.Security.Principal;
using System.Security.Claims;

Protector.Register("Alice", "Pa$$w0rd", roles: new string[] { "Admins" });
Protector.Register("Bob", "Pa$$w0rd", roles: new string[] { "Sales", "TeamLeads" });

Protector.Register("Eva", "Pa$$w0rd");

WriteLine($"Enter your username: ");
string? username = ReadLine();

WriteLine($"Enter your password: ");
string? password = ReadLine();

Protector.LogIn(username, password);

if (Thread.CurrentPrincipal == null)
{
    WriteLine("Log in failed.");
    return;
}

IPrincipal p = Thread.CurrentPrincipal;
WriteLine($"IsAuthenticated: {p.Identity.IsAuthenticated}");
WriteLine($"AuthenticationType: {p.Identity.AuthenticationType}");
WriteLine($"Name: {p.Identity.Name}");
WriteLine($"IsInRole(\"Admins\"): {p.IsInRole("Admins")}");
WriteLine($"IsRole(\"Sales\"): {p.IsInRole("Sales")}");

if (p is ClaimsPrincipal)
{
    WriteLine($"{p.Identity?.Name} has the following claims:");
    
    IEnumerable<Claim>? claims = (p as ClaimsPrincipal)?.Claims;

    if (claims is not null)
    {
        foreach (var c in claims)
        {
            WriteLine($"{c.Type} : {c.Value}");
        }
    }
}

try
{
    SecureFeature();
}
catch (Exception e)
{
    WriteLine($"{e.GetType()}: {e.Message}");
    throw;
}


static void SecureFeature()
{
    if (Thread.CurrentPrincipal == null)
    {
        throw new SecurityException("A user must be logged in to access this feature.");
    }
    
    if (!Thread.CurrentPrincipal.IsInRole("Admins"))
    {
        throw new SecurityException("Only users in the Admins role can access this feature.");
    }
    
    WriteLine("You have access to this feature.");
}