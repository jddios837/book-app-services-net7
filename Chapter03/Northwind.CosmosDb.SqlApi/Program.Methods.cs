using System.Net;
using Microsoft.Azure.Cosmos;

namespace Northwind.CosmosDb.SqlApi;

partial class Program
{
    private static string endPointUri = "";
    private static string primaryKey = "";

    static async Task CreateCosmosResource()
    {
        SectionTitle("Creating Cosmos Resource");

        try
        {
            using (CosmosClient client = new(accountEndpoint: endPointUri, authKeyOrResourceToken: primaryKey))
            {
                DatabaseResponse dbResponse = await client
                    .CreateDatabaseIfNotExistsAsync("Northwind", throughput: 400);

                string status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };
                
                WriteLine("Database Id: {0}, Status: {1}", arg0: dbResponse.Database.Id, arg1: status);

                IndexingPolicy indexingPolicy = new()
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true
                };
            }
        }
        catch (Exception e)
        {
            WriteLine(e);
            throw;
        }
    }

}