// See https://aka.ms/new-console-template for more information
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

string imageFolder = Path.Combine(Environment.CurrentDirectory, "images");

WriteLine($"Image folder {imageFolder}");
WriteLine();

if (!Directory.Exists(imageFolder))
{
    WriteLine();
    WriteLine("Folder does not exist!");
    return;
}

IEnumerable<string> images = Directory.EnumerateFiles(imageFolder);

foreach (string imagePath in images)
{
    if (Path.GetFileNameWithoutExtension(imagePath).EndsWith("-thumbnail"))
    {
        WriteLine($"Skipping:\n {imagePath}");
        WriteLine();
        continue; // this files has already been converted
    }

    var thumbnailPath = Path.Combine(
        Environment.CurrentDirectory, "images",
        Path.GetFileNameWithoutExtension(imagePath)
        + "-thumbnail" + Path.GetExtension(imagePath));

    using (Image image = Image.Load(imagePath))
    {
        WriteLine($"Converting:\n {imagePath}");
        WriteLine($"To:\n {thumbnailPath}");
        
        image.Mutate(x => x.Resize(image.Width / 10, image.Height / 10));
        image.Mutate(x => x.Grayscale());
        image.Save(thumbnailPath);
        WriteLine();
    }
}

WriteLine("Image processing complete. View the images folder.");