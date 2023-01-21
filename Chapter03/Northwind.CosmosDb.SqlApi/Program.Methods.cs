using System.Net;
using Microsoft.Azure.Cosmos;

using Packt.Shared;
using Northwind.CosmosDb.SqlApi.items;
using Microsoft.EntityFrameworkCore;

namespace Northwind.CosmosDb.SqlApi
{
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

        static async Task CreateProductItems()
        {
            SectionTitle("Creating product items");

            double totalCharge = 0.0;

            try
            {
                using (CosmosClient client = new(accountEndpoint: endPointUri, authKeyOrResourceToken: primaryKey))
                {
                    Container container = client.GetContainer("Northwind", containerId: "Products");

                    using (NorthwindContext db = new())
                    {
                        ProductCosmos[] products = db.Products
                            .Include(p => p.Category)
                            .Include(p => p.Supplier)
                            .Where(p => p.Category != null && p.Supplier != null)
                            .Select(p => new ProductCosmos
                            {
                                id = p.ProductId.ToString(),
                                productId = p.ProductId.ToString(),
                                productName = p.ProductName,
                                quantityPerUnit = p.QuantityPerUnit,
                                category = new CategoryCosmos
                                {
                                    categoryId = p.Category!.CategoryId,
                                    categoryName = p.Category!.CategoryName,
                                    descrition = p.Category!.Description
                                },
                                supplier = new SupplierCosmos
                                {
                                    supplierId = p.Supplier!.SupplierId,
                                    companyName = p.Supplier!.CompanyName,
                                    contactName = p.Supplier!.ContactName,
                                    contactTitle = p.Supplier!.ContactTitle,
                                    address = p.Supplier!.Address,
                                    city = p.Supplier!.City,
                                    region = p.Supplier!.Region,
                                    postalCode = p.Supplier!.PostalCode,
                                    country = p.Supplier!.Country,
                                    phone = p.Supplier!.Phone,
                                    fax = p.Supplier!.Fax,
                                    homePage = p.Supplier!.HomePage
                                },
                                unitPrice = p.UnitPrice,
                                unitsInStock = p.UnitsInStock,
                                unitsOnOrder = p.UnitsOnOrder,
                                reorderLevel = p.ReorderLevel,
                                discontinued = p.Discontinued,

                            })
                            .ToArray();

                        foreach (var p in products)
                        {
                            try
                            {
                                ItemResponse<ProductCosmos> productResponse =
                                    await container.ReadItemAsync<ProductCosmos>(
                                        id: p.id,
                                        new PartitionKey(p.productId));
                                WriteLine("Item with id: {0} exists Query consumed {1}",
                                    productResponse.Resource.id,
                                    productResponse.RequestCharge);

                                totalCharge += productResponse.RequestCharge;
                            }
                            catch (CosmosException ex)
                                when (ex.StatusCode == HttpStatusCode.NotFound)
                            {
                                ItemResponse<ProductCosmos> productResponse = await container.CreateItemAsync(p);
                                WriteLine("Created item with id: {0}, Insert consumed {1}",
                                    productResponse.Resource.id,
                                    productResponse.RequestCharge);

                                totalCharge += productResponse.RequestCharge;
                            }
                            catch (Exception ex)
                            {
                                WriteLine("Error: {0}, says {1}",
                                    ex.GetType(),
                                    ex.Message);
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                WriteLine("Error: {0}", ex.Message);
                WriteLine("Hint: Make sure the Azure Cosmos Emulator is running.");
            }
            catch (Exception ex)
            {
                WriteLine("Error: {0}, says {1}",
                    ex.GetType(),
                    ex.Message);
            }
            
            WriteLine("Total request charge: {0:N2} RUs", totalCharge);
        }

        static async Task ListProductItems(string sqlText = "SELECT * FROM c")
        {
            SectionTitle("Listing product items");

            try
            {
                using (CosmosClient client = new(accountEndpoint: endPointUri, authKeyOrResourceToken: primaryKey))
                {
                    Container container = client.GetContainer("Northwind", containerId: "Products");
                    
                    WriteLine("Running query {0}", sqlText);

                    QueryDefinition query = new(sqlText);

                    using FeedIterator<ProductCosmos> resultsIterator = container
                        .GetItemQueryIterator<ProductCosmos>(query);

                    if (!resultsIterator.HasMoreResults)
                    {
                        WriteLine("No results found");
                    }

                    while (resultsIterator.HasMoreResults)
                    {
                        FeedResponse<ProductCosmos> products =
                            await resultsIterator.ReadNextAsync();
                        
                        WriteLine("Status code: {0}, Request charge: {1} RUs",
                            products.StatusCode, products.RequestCharge);
                        
                        WriteLine("{0} Products found.", products.Count);

                        foreach (var product in products)
                        {
                            WriteLine("id: {0}, productName: {1}, unitPrice: {2}",
                                product.id, product.productName, product.unitPrice);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                WriteLine("Error: {0}", ex.Message);
                WriteLine("Hint: Make sure the Azure Cosmos Emulator is running.");
            }
            catch (Exception ex)
            {
                WriteLine("Error: {0}, says {1}",
                    ex.GetType(),
                    ex.Message);
            }
        }
        
        
    }
}