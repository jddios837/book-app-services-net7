// See https://aka.ms/new-console-template for more information

namespace Northwind.CosmosDb.SqlApi
{
    partial class Program
    { 
        static async Task Main(string[] args)
        {
            await CreateCosmosResource();
        }
    }
}