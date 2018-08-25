using System;
using System.Drawing;
using System.IO;

namespace CMK
{
    public class AnimatedPngCreator : IDisposable
    {
        public class Config
        {
            public bool? FilterUnchangedPixels { get; set; }
        }

        private readonly Creator creator;
        private readonly Config config;

        private ImageChangeAnalyser changeAnalyser;

        public AnimatedPngCreator(Stream stream, int x, int y, int defaultDelay = 500, int repeat = 0)
        {
            creator = new Creator(stream,x,y,defaultDelay,repeat);
            config = new Config { FilterUnchangedPixels = true };
            init();
        }

        private void init()
        {
            if (config.FilterUnchangedPixels == true)
                changeAnalyser = new ImageChangeAnalyser();
        }

        public AnimatedPngCreator(Stream stream, int x, int y, Config config, int defaultDelay = 500, int repeat = 0)
        {
            creator = new Creator(stream, x, y, defaultDelay, repeat);
            this.config = config;
        }

        public void WriteFrame(Image image, short frameDalay, int offsetX = 0, int offsetY = 0)
        {
            var img = config.FilterUnchangedPixels == true ?
                changeAnalyser.BlackoutImage(image) : image;
            creator.WriteFrame(img, frameDalay, offsetX, offsetY);
        }

        public void Dispose()
        {
            creator.Dispose();
        }
    }
}
