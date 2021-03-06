﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMK;
using System.IO;
using System.Drawing;

namespace AnimatedPngCreator.Console
{
    class Program
    {
        // Use the first argument for the output filename,
        // after that all the images to use in the animation.
        static void Main(string[] args)
        {
            if(args.Length > 2)
            {
                using (var output = File.Create(args[0]))
                {
                    int x=0, y=0;
                    using (Image firstImage = Image.FromFile(args[1]))
                    {
                        x = firstImage.Width;
                        y = firstImage.Height;
                    }
                    using (CMK.AnimatedPngCreator creator = new CMK.AnimatedPngCreator(output, x, y))
                    {
                        var imagePaths = args.Skip(1);
                        foreach (var imagePath in imagePaths)
                        {
                            using (Image image = Image.FromFile(imagePath))
                            {
                                creator.WriteFrame(image, 1000);
                            }
                        }
                    }                        
                }
            }
        }
    }
}
