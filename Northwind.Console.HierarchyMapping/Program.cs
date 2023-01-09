using Microsoft.EntityFrameworkCore;
using Northwind.Console.HierarchyMapping;

DbContextOptionsBuilder<HierarchyDb> options = new();
options.UseSqlServer("Data Source=localhost;Initial Catalog=HierarchyMapping;" +
                     "TrustServerCertificate=true;" +
                     "User Id=sa;Password=Tetra714217#;");

using (HierarchyDb db = new(options.Options))
{
    bool deleted = await db.Database.EnsureDeletedAsync();
    WriteLine($"Database deleted: {deleted}");

    bool created = await db.Database.EnsureCreatedAsync();
    WriteLine($"Database created: {created}");
    
    WriteLine("SQL script used tp create tje database:");
    WriteLine(db.Database.GenerateCreateScript());

    if (db.Students is null || !db.Students.Any())
    {
        WriteLine("There are no students.");
    }
    else
    {
        foreach (var s in db.Students)
        {
            WriteLine("{0} studies {1}", s.Name, s.Subject);
        }
    }
    
    if (db.Employees is null || !db.Employees.Any())
    {
        WriteLine("There are no employees.");
    }
    else
    {
        foreach (var e in db.Employees)
        {
            WriteLine("{0} studies {1}", e.Name, e.HireDate);
        }
    }
    
    if (db.People is null || !db.People.Any())
    {
        WriteLine("There are no people.");
    }
    else
    {
        foreach (var p in db.People)
        {
            WriteLine("{0} studies {1}", p.Name, p.Id);
        }
    }
}