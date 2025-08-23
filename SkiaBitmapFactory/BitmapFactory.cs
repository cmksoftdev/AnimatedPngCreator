using CMK.ExtendedBitmap;
using SkiaSharp;

namespace CMK.SkiaBitmapFactory
{
    public class BitmapWrapper : IBitmap, IDisposable
    {
        private SKBitmap bitmap;

        public uint Width => (uint)bitmap.Width;
        public uint Height => (uint)bitmap.Height;

        public BitmapWrapper(uint width, uint height)
        {
            bitmap = new SKBitmap((int)width, (int)height, SKColorType.Bgra8888, SKAlphaType.Premul);
        }

        public BitmapWrapper(SKBitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void Dispose() => bitmap.Dispose();

        public int[] GetPixelsAsIntArray()
        {
            int[] pixels = new int[bitmap.Width * bitmap.Height];
            var colors = bitmap.Bytes;
            for (int i = 0; i < pixels.Length; i++)
            {
                int idx = i * 4;
                byte b = colors[idx];
                byte g = colors[idx + 1];
                byte r = colors[idx + 2];
                byte a = colors[idx + 3];
                pixels[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }

            return pixels;
        }

        public void SetPixels(int[] pixels)
        {
            if (pixels.Length != bitmap.Width * bitmap.Height)
                throw new ArgumentException("Pixel-Array passt nicht zur Bitmap-Größe.");

            for (int i = 0; i < pixels.Length; i++)
                bitmap.SetPixel(i % bitmap.Width, i / bitmap.Width, new SKColor((uint)pixels[i]));
        }

        public void Save(MemoryStream stream)
        {
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(stream);
        }
    }

    public class BitmapFactory : IFactory
    {
        static BitmapFactory()
        {
            ExtendedBitmap.BitmapFactory._Create = (uint x, uint y) => new BitmapWrapper(x, y);
            ExtendedBitmap.BitmapFactory._FromFile = (string filePath) => new BitmapWrapper(SKBitmap.Decode(filePath));
        }
    }
}
