using Packt.Shared;

namespace Northwind.Common.EntityModels.Tests;

public class NorthwindEntityModelsTests
{
    [Fact]
    public void CanConnectIsTrue()
    {
        using NorthwindContext db = new();
        bool canConnect = db.Database.CanConnect(); // act
            
        Assert.True(canConnect);
    }

    [Fact]
    public void ProviderIsSqlServer()
    {
        using NorthwindContext db = new(); // arrange
        string? provider = db.Database.ProviderName; // act
            
        Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", provider); // assert
    }

    [Fact]
    public void ProductId1IsChai()
    {
        using NorthwindContext db = new();
        Product product1 = db.Products.Single(p => p.ProductId == 1);
        
        Assert.Equal("Chai", product1.ProductName);
    }

    [Fact]
    public void EmployeeHasLastRefreshedIn10sWindows()
    {
        NorthwindContext db = new(); // arrange
        Employee employee1 = db.Employees.Single(p => p.EmployeeId == 1); // act
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        Assert.InRange(actual: employee1.LastRefreshed,
            low: now.Subtract(TimeSpan.FromSeconds(5)),
            high: now.AddSeconds(5)); // assertion
    }
}