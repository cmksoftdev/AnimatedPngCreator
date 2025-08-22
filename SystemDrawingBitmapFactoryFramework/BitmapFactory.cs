using CMK.ExtendedBitmap;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CMK.SystemDrawingBitmapFactoryFramework
{
    public class BitmapWrapper : IBitmap
    {
        System.Drawing.Bitmap bitmap;
        public uint Width => (uint)bitmap.Width;
        public uint Height => (uint)bitmap.Height;

        public BitmapWrapper(uint width, uint height)
        {
            bitmap = new System.Drawing.Bitmap((int)width, (int)height);
        }

        public void Dispose()
        {
            bitmap.Dispose();
        }

        public int[] GetPixelsAsIntArray()
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new InvalidOperationException("Bitmap muss Format32bppArgb haben.");
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int[] pixels = new int[bitmap.Width * bitmap.Height];
            Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
            bitmap.UnlockBits(data);
            return pixels;
        }

        public void SetPixels(int[] pixels)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new InvalidOperationException("Bitmap muss Format32bppArgb haben.");
            if (pixels.Length != bitmap.Width * bitmap.Height)
                throw new ArgumentException("Pixel-Array passt nicht zur Bitmap-Größe.");
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bitmap.UnlockBits(data);
        }
    }

    public static class BitmapFactory
    {
        static BitmapFactory()
        {
            ExtendedBitmap.BitmapFactory._Create = (uint x, uint y) => new BitmapWrapper(x, y);
        }
    }
}
