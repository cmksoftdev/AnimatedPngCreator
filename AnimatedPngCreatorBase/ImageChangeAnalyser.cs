using CMK.ExtendedBitmap;

namespace CMK
{
    public class ImageChangeAnalyser
    {
        private IExtendedBitmap oldImage = null;

        private bool isB = false;
        public IExtendedBitmap BlackoutImage(IExtendedBitmap newImage)
        {
            uint s = 0;// isB ? 1 : 0;
            isB = !isB;
            var newImageBmp = newImage.Copy();
            var x = newImage.Width;
            var y = newImage.Height;
            var newImage2 = newImageBmp.Copy();
            if (oldImage != null)
            {
                for (uint i = 0; i < x - 4; i += 4)
                {
                    for (uint j = 0; j < y - 4; j += 4)
                    {
                        Color a = oldImage.GetPixel(i + s, j + s);
                        Color b = newImageBmp.GetPixel(i + s, j + s);
                        if (isColorEqual(a, b))
                        {
                            set_transparent(newImageBmp, i, j);
                        }
                    }
                }
            }
            oldImage = newImage2;
            return newImageBmp;
        }

        private void set_transparent(IExtendedBitmap bmp, uint x, uint y)
        {
            for (uint i = 0; i < 4; i++)
            {
                for (uint j = 0; j < 4; j++)
                {
                    bmp.SetPixel(i + x, j + y, Color.Transparent);
                }
            }
        }

        public IExtendedBitmap BlackoutImage(IExtendedBitmap newImage, out bool equal)
        {
            equal = false;
            if (newImage == null)
                return null;
            var newImageBmp = newImage.Copy();
            var x = newImage.Width;
            var y = newImage.Height;
            var pixelCount = x * y;
            var changeCount = 0;
            var newImage2 = newImageBmp.Copy();
            if (oldImage != null)
            {
                for (uint i = 0; i < x; i++)
                {
                    for (uint j = 0; j < y; j++)
                    {
                        Color a = oldImage.GetPixel(i, j);
                        Color b = newImageBmp.GetPixel(i, j);
                        if (isColorEqual(a, b))
                        {
                            changeCount++;
                            newImageBmp.SetPixel(i, j, Color.Transparent);
                        }
                    }
                }
            }
            oldImage = newImage2;
            equal = changeCount == pixelCount;
            return newImageBmp;
        }

        private bool isColorEqual(Color a, Color b) => a.R == b.R &&
                a.G == b.G &&
                a.B == b.B;
    }
}
