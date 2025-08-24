# AnimatedPngCreator
Creates animated PNG out ouf a sequence of Images.

Please use the newest version on NuGet.
<br>
[Get the package on NuGet](https://www.nuget.org/packages/AnimatedPngCreator/)

Dispose the creator to end the creating process. On disposing the count of images will be written to the header.

24.08.2025  v2.0.0 New project stucture: AnimatedPngCreator and AnimatedPngCreator.System.Drawing or AnimatedPngCreator.SkiaSharp is needed
<br>
12.10.2018  Bug fixed, tests added, new static methods added - v1.0.3 merged to master
<br>
29.8.2018  Bug fixed, test added, v1.0.2 deployed on NuGet
<br>
27.8.2018  Filter added to detect unchanged pixels, console app added, v1.0.1 deployed on NuGet

V1.0.3 makes it possible to use the static Create method. 

The third parameter is the time to show the frame, in milli seconds.


### Breaking Changes:

v2.0.0 Use BitmapFactory.FromFile instead of Image.FromFile and install AnimatedPngCreator.System.Drawing or AnimatedPngCreator.SkiaSharp


```csharp
// Example 1:
var filePaths = new List<string>
{
    "1.bmp", "2.bmp", "3.bmp"
};
CMK.AnimatedPngCreator.Create("out.png", filePaths, 1000);

// Example 2:
var images = new List<Image>
{
    BitmapFactory.FromFile("1.bmp"),
    BitmapFactory.FromFile("2.bmp"),
    BitmapFactory.FromFile("3.bmp")
};
CMK.AnimatedPngCreator.Create("out.png", images, 1000);

// Example 3:
var frames = new List<Frame>
{
    CMK.AnimatedPngCreator.Frame(BitmapFactory.FromFile("1.bmp"), 1000),
    CMK.AnimatedPngCreator.Frame(BitmapFactory.FromFile("2.bmp"), 1000),
    CMK.AnimatedPngCreator.Frame(BitmapFactory.FromFile("3.bmp"), 1000)
};
CMK.AnimatedPngCreator.Create("out.png", frames);
```

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
            Image image1 = BitmapFactory.FromFile("filename1.bmp");
            Image image2 = BitmapFactory.FromFile("filename2.jpg");
            Image image3 = BitmapFactory.FromFile("filename3.png");

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
                BitmapFactory.FromFile("filename1.bmp"),
                BitmapFactory.FromFile("filename2.jpg"),
                BitmapFactory.FromFile("filename3.png")
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
