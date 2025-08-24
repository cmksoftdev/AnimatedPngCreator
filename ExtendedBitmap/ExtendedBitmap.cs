using System;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CMK.ExtendedBitmap
{
    public interface IFactory {}

    public interface IBitmap : IDisposable
    {
        uint Width { get; }
        uint Height { get; }
        int[] GetPixelsAsIntArray();
        void SetPixels(int[] pixels);
        void Save(MemoryStream stream);
    }

    public static class BitmapFactory
    {
        public delegate IBitmap CreateBitmapDelegate(uint width, uint height);
        public delegate IBitmap CreateBitmapFromFileDelegate(string filepath);

        static BitmapFactory()
        {
            var assemblies = new[] { "SystemDrawingBitmapFactoryFramework.dll", "SystemDrawingBitmapFactoryCore.dll", "AnimatedPngCreator.SkiaSharp.dll" };
            var types = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .GetFiles()
                .Where(f => assemblies.Contains(f.Name))
                .Select(f => Assembly.LoadFrom(f.FullName))
                .SelectMany(a => {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        return ex.Types.Where(t => t != null);
                    }
                })
                .Where(t => t.IsClass && typeof(IFactory).IsAssignableFrom(t)).ToList();
            foreach (var type in types)
            {
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            }
        }

        public static CreateBitmapDelegate _Create { get; set; } = null;
        public static CreateBitmapFromFileDelegate _FromFile { get; set; } = null;

        internal static IBitmap CreateBitmap(uint width, uint height) => _Create?.Invoke(width, height) ?? throw new Exception("No Bitmapfactory set! Please use a valid package which includes a factory or implement one.");
        public static IExtendedBitmap Create(uint width, uint height) => new ExtendedBitmap(CreateBitmap(width, height));
        public static IExtendedBitmap FromFile(string filepath) => new ExtendedBitmap(_FromFile?.Invoke(filepath) ?? throw new Exception("No Bitmapfactory set! Please use a valid package which includes a factory or implement one."));
    }

    public class Color : IEquatable<Color>
    {
        public byte A { get; }
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Color(byte r, byte g, byte b) : this(255, r, g, b)
        {
        }

        public int ToArgb() => (A << 24) | (R << 16) | (G << 8) | B;

        public static Color FromArgb(int argb)
        {
            byte a = (byte)((argb >> 24) & 0xFF);
            byte r = (byte)((argb >> 16) & 0xFF);
            byte g = (byte)((argb >> 8) & 0xFF);
            byte b = (byte)(argb & 0xFF);
            return new Color(a, r, g, b);
        }

        public bool Equals(Color other)
        {
            if (other is null) return false;
            return A == other.A && R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals(object obj) => obj is Color other && Equals(other);

        public override int GetHashCode() => ToArgb();

        public static bool operator ==(Color left, Color right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(Color left, Color right) => !(left == right);

        public override string ToString() => $"Color [A={A}, R={R}, G={G}, B={B}]";

        public static Color Transparent { get; } = new Color(0, 0, 0, 0);
        public static Color Black { get; } = new Color(255, 0, 0, 0);
        public static Color White { get; } = new Color(255, 255, 255, 255);
    }

    public class ExtendedBitmap : IExtendedBitmap
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
            Bitmap = BitmapFactory.CreateBitmap(width, height);
            this.pixels = pixels;
            SaveAllChanges();
        }

        public ExtendedBitmap(uint width, uint height)
        {
            Width = width;
            Height = height;
            Bitmap = BitmapFactory.CreateBitmap(width, height);
            this.pixels = new int[width * height];
            SaveAllChanges();
        }

        public ExtendedBitmap(uint width, uint height, Color color)
        {
            Width = width;
            Height = height;
            Bitmap = BitmapFactory.CreateBitmap(width, height);
            this.pixels = new int[width * height];
            for (uint y = 0; y < Height; y++)
            {
                for (uint x = 0; x < Width; x++)
                {
                    SetPixel(x, y, color);
                }
            }
            SaveAllChanges();
        }

        public IExtendedBitmap Copy() => new ExtendedBitmap(Width, Height, pixels.Clone() as int[]);

        public Color GetPixel(uint x, uint y) => Color.FromArgb(pixels[x + y * Width]);

        public int GetPixelAsInt(uint x, uint y) => pixels[x + y * Width];

        public void SetPixel(uint x, uint y, Color color) => SetPixel(x, y, color.ToArgb());

        public void SetPixel(uint x, uint y, int color) => pixels[x + y * Width] = color;

        public void Refresh() => pixels = Bitmap.GetPixelsAsIntArray();

        public void SaveAllChanges() => Bitmap.SetPixels(pixels);

        public void Save(MemoryStream stream)
        {
            SaveAllChanges();
            Bitmap.Save(stream);
        }

        public void Dispose()
        {
            if (dispose)
                Bitmap.Dispose();
        }
    }
}
