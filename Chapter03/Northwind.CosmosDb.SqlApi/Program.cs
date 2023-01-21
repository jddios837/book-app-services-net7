// See https://aka.ms/new-console-template for more information

namespace Northwind.CosmosDb.SqlApi
{
    partial class Program
    { 
        static async Task Main(string[] args)
        {
            //await CreateCosmosResource();
            await CreateProductItems();
            await ListProductItems("SELECT p.id, p.productName, p.unitPrice FROM p WHERE p.category.categoryName = 'Beverages'");
            //await DeletedProductItems();
        }
    }
}