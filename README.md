# AnimatedPngCreator
Creates animated PNG out ouf a sequence of Images.

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

            short frameDalay = 1000 / 5; //5 frames per second
            using (FileStream outputFile = File.Create("animated.png"))
            {
                using (var apngCreator = new AnimatedPngCreator(outputFile, image1.Width, image1.Height))
                {
                    apngCreator.WriteFrame(image1, frameDalay);
                    apngCreator.WriteFrame(image2, frameDalay);
                    apngCreator.WriteFrame(image3, frameDalay);
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

            short frameDalay = 1000 / 5; //5 frames per second
            using (FileStream outputFile = File.Create("animated.png"))
            {
                using (var apngCreator = new AnimatedPngCreator(outputFile, images[0].Width, images[0].Width))
                {
                    foreach(var image in images)
                    {
                        apngCreator.WriteFrame(image, frameDalay);
                    }
                }
            }
        }
    }
}
```
