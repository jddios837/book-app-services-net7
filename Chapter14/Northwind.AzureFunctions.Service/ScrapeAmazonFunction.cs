using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Northwind.AzureFunctions.Service;

public class ScrapeAmazonFunction
{
    private const string relativePath = "10-NET-Cross-Platform-Development-websites/dp/1801077363/";

    private readonly IHttpClientFactory clientFactory;
        
    public ScrapeAmazonFunction(IHttpClientFactory clientFactory)
    {
        this.clientFactory = clientFactory;
    }
    
    [FunctionName(nameof(ScrapeAmazonFunction))]
    public async Task RunAsync([TimerTrigger("*/120 * * * * *")] TimerInfo timer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        log.LogInformation(
            "C# Timer trigger function next three occurrences at: " +
            $"{timer.FormatNextOccurrences(3, System.DateTime.UtcNow)}.");
        
        var client = clientFactory.CreateClient("Amazon");
        var response = await client.GetAsync(relativePath);
        if (response.IsSuccessStatusCode)
        {
            log.LogInformation($"Successful HTTP request with Status Code: {response.StatusCode}");
            
            Stream steam = await response.Content.ReadAsStreamAsync();
            GZipStream gZipStream = new(steam, CompressionMode.Decompress);
            StreamReader reader = new(gZipStream);
            string page = reader.ReadToEnd();
            
            // extract best seller rank
            int posBsr = page.IndexOf("Best Sellers Rank");
            string bsrSection = page.Substring(posBsr, 45);
            
            // bsrSection will be something like:
            // "Best Sellers Rank: </span> #22,258 in Books ("
            // get the position of the # and the following space
            int posHash = bsrSection.IndexOf("#") + 1;
            int posSpaceAfterHash = bsrSection.IndexOf(" ", posHash);
            
            // get the BSR number as text
            string bsr = bsrSection.Substring(posHash, posSpaceAfterHash - posHash);
            bsr = bsr.Replace(",", null);
            
            // parse the text into a number
            if (int.TryParse(bsr, out int bestSellersRank))
            {
                log.LogInformation($"Best Sellers Rank: {bestSellersRank:N0}");
            }
            else
            {
                log.LogError($"Unable to parse Best Sellers Rank number from: {bsrSection}");
            }
        }
        else
        {
            log.LogError($"Bad HTTP request with Error: {response.StatusCode}");
        }
        
        
    }
}