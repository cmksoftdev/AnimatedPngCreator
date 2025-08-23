using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace CMK.ExtendedBitmap
{
    public interface IExtendedBitmap : IDisposable
    {
        IBitmap Bitmap { get; }
        uint Height { get; }
        uint PixelCount { get; }
        uint Width { get; }

        void Dispose();
        Color GetPixel(uint x, uint y);
        int GetPixelAsInt(uint x, uint y);
        void Refresh();
        void SaveAllChanges();
        void SetPixel(uint x, uint y, Color color);
        void SetPixel(uint x, uint y, int color);
        void Save(MemoryStream stream);
        IExtendedBitmap Copy();
    }
}