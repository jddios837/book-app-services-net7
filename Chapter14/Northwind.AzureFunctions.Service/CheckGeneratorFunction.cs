using System.Threading.Tasks;
using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing; // Mutate extension method

namespace Northwind.AzureFunctions.Service;

[StorageAccount("AzureWebJobsStorage")]
public static class CheckGeneratorFunction
{
    [FunctionName(nameof(CheckGeneratorFunction))]
    public static async Task RunAsync(
        [QueueTrigger("checksQueue", Connection = "")] QueueMessage message,
        [Blob("checks-blob-container")] BlobContainerClient blobContainerClient,
        ILogger log)
    {
       // wrote some information about the message to the log
       log.LogInformation("C# Queue trigger function executed.");
       log.LogInformation($"MessageId: {message.MessageId}.");
       log.LogInformation($"InsertedOn: {message.InsertedOn}.");
       log.LogInformation($"ExperesOn: {message.ExpiresOn}.");
       log.LogInformation($"Body: {message.Body}.");

       // create a mew blank image with a white background
       using (Image<Rgba32> image = new(width:1200, height: 600, backgroundColor: new Rgba32(r: 255, g: 255, b: 255, a: 100)))
       {
           // load the font file and create a large font
           FontCollection collection = new();
           FontFamily fontFamily = collection.Add(@"fonts\Caveat\static\Caveat-Regular.ttf");
           Font font = fontFamily.CreateFont(72);
           
           string amount = message.Body.ToString();

           DrawingOptions options = new()
           {
               GraphicsOptions = new()
               {
                   ColorBlendingMode = PixelColorBlendingMode.Multiply
               }
           };
           
           // define some pens and brushes
           var blackPen = Pens.Solid(Color.Black, 2);
           var blackThickPen = Pens.Solid(Color.Black, 8);
           var greenPen = Pens.Solid(Color.Green, 3);
           var redBrush = Brushes.Solid(Color.Red);
           var blueBrush = Brushes.Solid(Color.Blue);
           
           // define some paths and draw them
           var border = new RectangularPolygon(x: 50, y: 50, width: 1100, height: 500);
           image.Mutate(x => x.Draw(options, blackPen, border));

           IPath start = new Star(x: 150.0f, y: 150.0f, prongs: 5, innerRadii: 20.0f, outerRadii: 30.0f);
           
           image.Mutate(x => x.Fill(options, redBrush, start).Draw(options, greenPen, start));
           
           IPath line1 = new Polygon(new LinearLineSegment(
               new PointF(x: 100, y: 275), new PointF(x: 1050, y: 275)));
           
           image.Mutate(x => x.Draw(options, blackPen, line1));
           IPath line2 = new Polygon(new LinearLineSegment(
               new PointF(x: 100, y: 365), new PointF(x: 1050, y: 365)));
           
           image.Mutate(x => x.Draw(options, blackPen, line2));
           
           RichTextOptions textOptions = new(font)
           {
               Origin = new PointF(100, 200),
               WrappingLength = 1000,
               HorizontalAlignment = HorizontalAlignment.Left
           };
           
           image.Mutate(x => x.DrawText(textOptions, amount, blueBrush, blackPen));

           string blobName = $"{System.DateTime.UtcNow:yyyy-MM-dd-hh-mm-ss}.png";
           
           log.LogInformation($"Blob name: {blobName}");

           try
           {
               if (Environment.GetEnvironmentVariable("IS_LOCAL") == "true")
               {
                   // create blob in the local filesystem
                   string folder = $@"{System.Environment.CurrentDirectory}\blobs";

                   if (!Directory.Exists(folder))
                   {
                       Directory.CreateDirectory(folder);
                   }
                   
                   log.LogInformation($"Blobs folder: {folder}");

                   string blobPath = $@"{folder}\{blobName}";
                   
                   await image.SaveAsPngAsync(blobPath);
               }
               
               // create BLOB in Blob Storage via a memory stream

               Stream stream = new MemoryStream();

               await image.SaveAsPngAsync(stream);
               stream.Seek(0, SeekOrigin.Begin);

               blobContainerClient.CreateIfNotExists();
               
               BlobContentInfo info = await blobContainerClient.UploadBlobAsync(blobName, stream);
               
               log.LogInformation($"Blob sequence number: {info.BlobSequenceNumber}.");
           }
           catch (Exception e)
           {
               log.LogError(e.Message);
           }
       } 
    }
}