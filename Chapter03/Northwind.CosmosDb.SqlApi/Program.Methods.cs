using System.Net;
using Microsoft.Azure.Cosmos;

namespace Northwind.CosmosDb.SqlApi;

partial class Program
{
    private static string endPointUri = "https://localhost:8081/";
    private static string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

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
                    Automatic = true,
                    IncludedPaths = { new IncludedPath { Path = "/*" } }
                };

                ContainerProperties containerProperties = new("Products",
                    partitionKeyPath: "/productId")
                {
                    IndexingPolicy = indexingPolicy
                };

                ContainerResponse containerResponse = await dbResponse.Database
                    .CreateContainerIfNotExistsAsync(containerProperties: containerProperties, throughput: 1000);

                status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };

                WriteLine("Container Id: {0}, Status: {1}",
                    arg0: containerResponse.Container.Id, arg1: status);

                Container container = containerResponse.Container;

                ContainerProperties properties = await container.ReadContainerAsync();

                WriteLine($" PartitionKeyPath: {properties.PartitionKeyPath}");
                WriteLine($" LastModified: {properties.LastModified}");
                WriteLine($" PartitionKeyPath: {properties.IndexingPolicy.IndexingMode}");
                WriteLine($" IndexingPolicy.IndexingMode: {properties.IndexingPolicy.IndexingMode}");
                WriteLine($" IndexingPolicy.IncludePaths: {0}",
                    string.Join(",", properties.IndexingPolicy.IncludedPaths.Select(p => p.Path)));
                WriteLine($" IndexingPolicy: {properties.IndexingPolicy}");
            }
        }
        catch (HttpRequestException e)
        {
            WriteLine($"Error: {0}", e.Message);
            WriteLine("Hint: make sure the azure cosmos emulator is running.");
        }
        catch (Exception e)
        {
            WriteLine("Error {0} says {1}", e.GetType(), e.Message);
        }
    }

}