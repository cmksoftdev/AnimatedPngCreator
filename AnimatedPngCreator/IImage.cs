using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimatedPngCreator
{
    public interface IImage
    {
        uint Width { get; }
        uint Height { get; }

        int GetPixelAsInt(int x, int y);
    }
}
