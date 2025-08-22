using System;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace CMK.ExtendedBitmap
{
    public interface IBitmap : IDisposable
    {
        uint Width { get; }
        uint Height { get; }
        int[] GetPixelsAsIntArray();
        void SetPixels(int[] pixels);
    }

    public static class BitmapFactory
    {
        public delegate IBitmap CreateBitmap(uint width, uint height);
        public delegate IBitmap CreateBitmapFromFile(string filepath);
        public static CreateBitmap _Create { get; set; } = null;
        public static CreateBitmapFromFile _CreateFromFile { get; set; } = null;
        public static IBitmap Create(uint width, uint height) => _Create?.Invoke(width, height) ?? throw new Exception("No Bitmapfactory set! Please use a valid package which includes a factory or implement one.");
        public static IBitmap Create(string filepath) => _CreateFromFile?.Invoke(filepath) ?? throw new Exception("No Bitmapfactory set! Please use a valid package which includes a factory or implement one.");

    }

    public class ExtendedBitmap : IDisposable
    {
        public IBitmap Bitmap { get; }
        public uint Width { get; }
        public uint Height { get; }
        public uint PixelCount => (uint)pixels.Length;

        int[] pixels;
        readonly bool dispose;

        public ExtendedBitmap(IBitmap bitmap, bool dispose = false)
        {
            this.Bitmap = bitmap;
            Width = bitmap.Width;
            Height = bitmap.Height;
            pixels = bitmap.GetPixelsAsIntArray();
            this.dispose = dispose;
        }

        public ExtendedBitmap(uint width, uint height, int[] pixels)
        {
            Width = width;
            Height = height;
            Bitmap = BitmapFactory.Create(width, height);
            this.pixels = pixels;
            SaveAllChanges();
        }

        public ExtendedBitmap(uint width, uint height)
        {
            Width = width;
            Height = height;
            Bitmap = BitmapFactory.Create(width, height);
            this.pixels = new int[width * height];
            SaveAllChanges();
        }

        public ExtendedBitmap(uint width, uint height, Color color)
        {
            Width = width;
            Height = height;
            Bitmap = BitmapFactory.Create(width, height);
            this.pixels = new int[width * height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetPixel(x, y, color);
                }
            }
            SaveAllChanges();
        }

        public Color GetPixel(uint x, uint y)
        {
            return Color.FromArgb(pixels[x + y * Width]);
        }

        public int GetPixelAsInt(uint x, uint y)
        {
            return pixels[x + y * Width];
        }

        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.ToArgb());
        }

        public void SetPixel(int x, int y, int color)
        {
            pixels[x + y * Width] = color;
        }

        public void Refresh()
        {
            pixels = Bitmap.GetPixelsAsIntArray();
        }

        public void SaveAllChanges()
        {
            Bitmap.SetPixels(pixels);
        }

        public void Dispose()
        {
            if (dispose)
                Bitmap.Dispose();
        }
    }
}
