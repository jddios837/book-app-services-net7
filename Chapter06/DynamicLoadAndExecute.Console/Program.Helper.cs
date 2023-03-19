// See https://aka.ms/new-console-template for more information
partial class Program
{
    static void OutputAssemblyInfo(Assembly a)
    {
        WriteLine("FullName: {0}", a.FullName);
        WriteLine("Location: {0}", Path.GetDirectoryName(a.Location));
        WriteLine("IsCollectible: {0}", a.IsCollectible);
        WriteLine("Defined types:");
        foreach (var info in a.DefinedTypes)
        {
            if (!info.Name.EndsWith("Attribute"))
            {
                WriteLine("    Name: {0}, Members: {1}", info.Name, info.GetMembers().Length);
            }
        }
        WriteLine();
    }
}