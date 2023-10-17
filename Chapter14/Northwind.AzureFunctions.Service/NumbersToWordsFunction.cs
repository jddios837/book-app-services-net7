﻿using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Packt.Shared;

namespace Northwind.AzureFunctions.Service;

[StorageAccount("AzureWebJobsStorage")]
public static class NumbersToWordsFunction
{
    [FunctionName("NumbersToWordsFunction")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        [Queue("checksQueue")] ICollector<string> collector, 
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        string amount = req.Query["amount"];
        if (BigInteger.TryParse(amount, out BigInteger number))
        {
            string words = number.ToWords();
            collector.Add(words);
            return await Task.FromResult(new OkObjectResult(words));
        }
        else
        {
            return new BadRequestObjectResult($"Failed to parse: {amount}");
        }
    }
}