// See https://aka.ms/new-console-template for more information
// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching

Console.WriteLine("Hello, World!");

var checkValueString = Guid.Parse("16732FCC-DB39-4512-8E59-3E2FE7C3679B").ToString().ToUpper();
var checkGuid = Guid.Parse("4E23F082-B62B-4901-8CC2-FE909EBD1140");

switch (checkGuid)
{
    case var r when r == ResourceType.ManagementRole:
        Console.WriteLine("MANAGEMENT_ROLE");
        return;
    case var r when r == ResourceType.GroupGeneric: // pattern matching
        Console.WriteLine("GROUP_GENERIC");
        return;
    case var b when b == ResourceType.GroupDistribution:
        Console.WriteLine("GROUP_GENERIC GROUP_DISTRIBUTION");
        return;
}




public static class ResourceType
{
    // convert these string in Guids
    public static readonly Guid ManagementRole = Guid.Parse("16732FCC-DB39-4512-8E59-3E2FE7C3679B");
    public static readonly Guid GroupGeneric = Guid.Parse("4E23F082-B62B-4901-8CC2-FE909EBD1140");
    public static readonly Guid GroupDistribution = Guid.Parse("C2429206-DDC3-4762-850C-015290A96543");
    public static readonly Guid GroupSecurity = Guid.Parse("9276CE73-E037-49CA-B2B8-32E0E6C39DF2");
    public static readonly Guid WindowsFileShare = Guid.Parse("CE40F0F7-9687-4D60-BE97-C728E05266B4");
    public static readonly Guid ProtectedApplication = Guid.Parse("664C53C5-7FEB-4C07-BCCE-F2B0148B434D");
    public static readonly Guid AzureApplication = Guid.Parse("963D322C-4E3C-48FC-874B-B2843C4B85E4");
    public static readonly Guid Mailbox = Guid.Parse("4CA58C0B-B0F9-49CB-BE15-99D70CB49B2C");
    public static readonly Guid Person = Guid.Parse("1CDBE8F2-A59E-4FB6-A813-43A2BCA0E769");
    public static readonly Guid Account = Guid.Parse("C3B6388C-5C36-40F6-AD1F-C1FD16144D3D");
}

