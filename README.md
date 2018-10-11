# AnimatedPngCreator
Creates animated PNG out ouf a sequence of Images.

Please use NuGet package v1.0.2 because there is a bug in v1.0.1.
<br>
[Get the package on NuGet](https://www.nuget.org/packages/AnimatedPngCreator/)

Dispose the creator to end the creating process. On disposing the count of images will be written to the header.

12.10.2018  Bug Fixed, Tests added, new static methods added - v1.0.3 merged to master
<br>
29.8.2018  Bug Fixed, Test added, v1.0.2 deployed on NuGet
<br>
27.8.2018  Filter added to detect unchanged pixels, console app added, v1.0.1 deployed on NuGet


The filter to remove unchanged pixels is on by default. If you don't want to use the filter, you just have to pass a config to the constructor:

```csharp
var config = new AnimatedPngCreator.Config
{
    FilterUnchangedPixels = false
};
using (var apngCreator = new AnimatedPngCreator(outputFile, image1.Width, image1.Height, config))
```

Example 1:

```csharp
// Add three images to an APNG
using CMK;
using System.Drawing;
using System.IO;

namespace ApngTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Image image1 = Image.FromFile("filename1.bmp");
            Image image2 = Image.FromFile("filename2.jpg");
            Image image3 = Image.FromFile("filename3.png");

            short frameDelay = 1000 / 5; //5 frames per second
            using (FileStream outputFile = File.Create("animated.png"))
            {
                using (var apngCreator = new AnimatedPngCreator(outputFile, image1.Width, image1.Height))
                {
                    apngCreator.WriteFrame(image1, frameDelay);
                    apngCreator.WriteFrame(image2, frameDelay);
                    apngCreator.WriteFrame(image3, frameDelay);
                }
            }
        }
    }
}
```
Example 2:
```csharp
using CMK;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ApngTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Image> images = new List<Image>
            {
                Image.FromFile("filename1.bmp"),
                Image.FromFile("filename2.jpg"),
                Image.FromFile("filename3.png")
            };

            short frameDelay = 1000 / 5; //5 frames per second
            using (FileStream outputFile = File.Create("animated.png"))
            {
                using (var apngCreator = new AnimatedPngCreator(outputFile, images[0].Width, images[0].Height))
                {
                    foreach(var image in images)
                    {
                        apngCreator.WriteFrame(image, frameDelay);
                    }
                }
            }
        }
    }
}
```
