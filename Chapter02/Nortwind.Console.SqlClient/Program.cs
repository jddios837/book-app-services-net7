// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

SqlConnectionStringBuilder builder = new();
builder.InitialCatalog = "Northwind";
builder.MultipleActiveResultSets = true;
builder.Encrypt = true;
builder.ConnectTimeout = 10;
builder.TrustServerCertificate = true; // This line solve error in the login proccess

WriteLine("Connect to:");
WriteLine("1 - SQL Server on local machine");
WriteLine("2 - Azure SQL Database");
WriteLine("3 - Azure SQL Edge");
WriteLine();
Write("Press a key: ");

ConsoleKey key = ReadKey().Key;
WriteLine(); WriteLine();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.DataSource = "localhost"; // Local SQL Server
    // @".\newt7book"; // Local SQL Server with an instance name
} else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    builder.DataSource = "tcp:apps-services-net7.database.windows.net,1433";
} else if (key is ConsoleKey.D3 or ConsoleKey.NumPad3)
{
    builder.DataSource = "tcp:127.0.0.1,1433"; // Azure SQL Edge
}
else
{
    WriteLine("No data source selected.");
    return;
}

WriteLine("Authenticate using:");
WriteLine("1 - Windows Integrated Security");
WriteLine("2 - SQL Login, for example, sa");
WriteLine();
Write("Press a key: ");

key = ReadKey().Key;
WriteLine(); WriteLine();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    builder.IntegratedSecurity = true;
} else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    builder.UserID = "sa";
    
    Write("Enter your SQL Server password: ");
    string? password = ReadLine();
    if (string.IsNullOrWhiteSpace(password))
    {
        WriteLine("Password cannot be empty or null.");
        return;
    }

    builder.Password = password;
    builder.PersistSecurityInfo = false;
}
else
{
    WriteLine("No authentication selected.");
    return;
}

SqlConnection connection = new(builder.ConnectionString);
WriteLine(connection.ConnectionString);
WriteLine();

connection.StateChange += Connection_StateChange;
connection.InfoMessage += Connection_InfoMessage;

try
{
    WriteLine("Opening connection. Please wait up to {0} seconds...", builder.ConnectTimeout);
    WriteLine();
    // connection.Open();
    await connection.OpenAsync();

    WriteLine($"SQL Server version: {connection.ServerVersion}");

    connection.StatisticsEnabled = true;
}
catch (SqlException ex)
{
    WriteLine($"SQL exception: {ex.Message}");
    return;
}

// Executing a Query
SqlCommand cmd = connection.CreateCommand();

Console.Write("Enter a unit price: ");
string? priceText = ReadLine();

if (!decimal.TryParse(priceText, out decimal price))
{
    WriteLine("You must enter a valid unit price.");
    return;
}

WriteLine("Execute command using :");
WriteLine("   1 - Text");
WriteLine("   2 - Stored Procedure");
WriteLine();
Write("Press a Key: ");

key = ReadKey().Key;
WriteLine(); WriteLine();

SqlParameter p1, p2 = new(), p3 = new();

if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
{
    cmd.CommandType = CommandType.Text;
    cmd.CommandText = "SELECT ProductId, ProductName, UnitPrice FROM Products"
                      + " WHERE UnitPrice > @price";
    cmd.Parameters.AddWithValue("price", price);
}
else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
{
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.CommandText = "GetExpensiveProducts";

    p1 = new()
    {
        ParameterName = "price",
        SqlDbType = SqlDbType.Money,
        SqlValue = price
    };
    
    p2 = new()
    {
        Direction = ParameterDirection.Output,
        ParameterName = "count",
        SqlDbType = SqlDbType.Int
    };
    
    p3 = new()
    {
        Direction = ParameterDirection.ReturnValue,
        ParameterName = "rv",
        SqlDbType = SqlDbType.Int
    };

    cmd.Parameters.Add(p1);
    cmd.Parameters.Add(p2);
    cmd.Parameters.Add(p3);
}

// SqlDataReader r = cmd.ExecuteReader();
SqlDataReader r = await cmd.ExecuteReaderAsync();

Console.WriteLine("------------------------------------------------------------");
Console.WriteLine("| {0, 5} | {1, -35} | {2, 8} |", "Id", "Name", "Price");
Console.WriteLine("------------------------------------------------------------");

// while (r.Read())
while (await r.ReadAsync())
{
    Console.WriteLine("| {0, 5} | {1, -35} | {2,8:C} |",
        await r.GetFieldValueAsync<int>("ProductId"),
        await r.GetFieldValueAsync<string>("ProductName"),
        await r.GetFieldValueAsync<decimal>("UnitPrice"));
        // r.GetInt32("ProductId"),
        // r.GetString("ProductName"),
        // r.GetDecimal("UnitPrice"));
}

await r.CloseAsync();

WriteLine($"Output count: {p2.Value}");
WriteLine($"Retun value: {p3.Value}");

await connection.CloseAsync();
