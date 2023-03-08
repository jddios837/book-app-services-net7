// See https://aka.ms/new-console-template for more information
using System.Reflection; // Assembly
using Packt.Shared; // CoderAttribute

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

AssemblyInformationalVersionAttribute? version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
WriteLine($" Version: {version?.InformationalVersion}");

AssemblyCompanyAttribute? company = assembly
    .GetCustomAttribute<AssemblyCompanyAttribute>();
WriteLine($" Company: {company?.Company}");

IEnumerable<Attribute> attributes = assembly.GetCustomAttributes();
WriteLine($" Assembly-level attributes:");
foreach (Attribute attribute in attributes)
{
    WriteLine($"         {attribute.GetType()}");
}

// Types
WriteLine();
WriteLine($"* Types:");
Type[] types = assembly.GetTypes();

foreach (Type t in types)
{
    WriteLine();
    WriteLine($"Type: {t.FullName}");
    MemberInfo[] members = t.GetMembers();

    foreach (MemberInfo m in members)
    {
        ObsoleteAttribute? obsolete = m.GetCustomAttribute<ObsoleteAttribute>();
        
        WriteLine("{0}: {1} ({2}) {3}", 
            m.MemberType, 
            m.Name, 
            m.DeclaringType?.Name, 
            obsolete is null ? "" : $"Obsolete! {obsolete.Message}");

        IOrderedEnumerable<CoderAttribute> coders =
            m.GetCustomAttributes<CoderAttribute>().OrderByDescending(c => c.LastModified);

        foreach (CoderAttribute coder in coders)
        {
            WriteLine("-> Modify by {0} on {1}", coder.Coder, coder.LastModified.ToShortDateString());
        }
    }
}
