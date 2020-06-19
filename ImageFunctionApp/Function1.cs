using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ImageFunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run(
            //First argument will always be a trigger.
            [BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, 
            [Blob("thumbnails/{name}",FileAccess.Write)] Stream outputBlob,
            string name, 
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using(Image<Rgba32> image = Image.Load<Rgba32>(myBlob))
            {
                image.Mutate(i => i.Resize(new ResizeOptions { Size = new Size(250, 250), Mode=ResizeMode.Max }).Grayscale());
                image.Save(outputBlob, new JpegEncoder());
            }          
        }
    }
}
