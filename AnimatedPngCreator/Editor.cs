using CMK.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CMK.DTOs.AnimatedPng;

namespace CMK
{
    internal class Editor : EngineBase
    {
        private AnimatedPng animatedPng;

        public Editor(string filePath)
        {
            animatedPng = new AnimatedPng
            {
                FilePath = filePath,
                Frames = new List<Frame>()
            };
            load();
        }

        public Editor(Stream stream, string filePath)
        {
            animatedPng = new AnimatedPng
            {
                FilePath = filePath,
                Frames = new List<Frame>()
            };
            load(stream);
        }

        private void load()
        {
            var stream = File.OpenRead(animatedPng.FilePath);
            load(stream);
        }

        private void load(Stream stream)
        {

            stream.Dispose();
        }

        private Frame getFrame()
        {
            return null;
        }
    }
}
