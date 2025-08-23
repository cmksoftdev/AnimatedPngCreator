using CMK.ExtendedBitmap;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CMK.SystemDrawingBitmapFactoryCore
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

        public BitmapWrapper(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void Dispose() => bitmap.Dispose();

        public static Bitmap ChangeBitmapFormat(Bitmap original, PixelFormat newFormat)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, newFormat);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height));
            }
            return newBitmap;
        }

        public int[] GetPixelsAsIntArray()
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                bitmap = ChangeBitmapFormat(bitmap, PixelFormat.Format32bppArgb);
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
                bitmap = ChangeBitmapFormat(bitmap, PixelFormat.Format32bppArgb);
            if (pixels.Length != bitmap.Width * bitmap.Height)
                throw new ArgumentException("Pixel-Array passt nicht zur Bitmap-Größe.");
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bitmap.UnlockBits(data);
        }

        public void Save(MemoryStream stream) => bitmap.Save(stream, ImageFormat.Png);
    }

    public class BitmapFactory : IFactory
    {
        static BitmapFactory()
        {
            CMK.ExtendedBitmap.BitmapFactory._Create = (uint x, uint y) => new BitmapWrapper(x, y);
            CMK.ExtendedBitmap.BitmapFactory._FromFile = (string filePath) => new BitmapWrapper((Bitmap)Image.FromFile(filePath));
        }
    }
}
