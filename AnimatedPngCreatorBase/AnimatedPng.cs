using CMK.ExtendedBitmap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMK
{
    public class AnimatedPng
    {
        internal abstract class Chunk
        {
            public abstract byte[] ToByteArray();

            public void Write(Stream stream)
            {
                var bytes = ToByteArray();
                stream.Write(bytes, 0, bytes.Length);
            }

            protected void validateData(byte[] array, int fullLength, string name)
            {
                if (array.Length != fullLength)
                    throw new Exception($"Invalid {name} array length. It has to be {fullLength} bytes.");
                int contentLength = fullLength - 12;
                int length = EngineBase.getSwappedInt(array, 0);
                if (length != contentLength)
                    throw new Exception($"Invalid {name} content length. It has to be {contentLength} bytes.");
                string _name = "";
                for (int i = 5; i < 9; i++)
                    _name += array[i];
                if (_name != name)
                    throw new Exception($"Invalid {name}. It has to be \"{name}\".");
                var expectedCrc = EngineBase.skipLengthPropertyAndGetCrc(array);
                var actualCrc = EngineBase.getSwappedUInt(array, array.Length - (4 + length));
                if(actualCrc != expectedCrc)
                    throw new Exception($"Invalid {name}. Checksumme is invalid.");
            }
        }

        public class Frame
        {
            public IExtendedBitmap Image { get; set; }
            public short Delay { get; set; }
        }

        internal class acTL : Chunk
        {
            public uint num_frames { get; set; }
            public uint num_plays { get; set; }

            public acTL(byte[] array)
            {
                validateData(array, 20, "acTL");
                num_frames = EngineBase.getSwappedUInt(array, 8);
                num_plays = EngineBase.getSwappedUInt(array, 12);
            }

            public override byte[] ToByteArray()
            {
                var array = new byte[20];
                EngineBase.getSwappedArray(8).CopyTo(array, 0);
                Encoding.Default.GetBytes("acTL").CopyTo(array, 4);
                EngineBase.getSwappedArray((int)num_frames).CopyTo(array, 8);
                EngineBase.getSwappedArray((int)num_plays).CopyTo(array, 12);
                EngineBase.getSwappedCrc(array.Skip(4).Take(12).ToArray()).CopyTo(array, 16);
                return array;
            }
        }

        internal class fcTL : Chunk
        {
            public uint sequence_number { get; set; }
            public uint width { get; set; }
            public uint height { get; set; }
            public uint x_offset { get; set; }
            public uint y_offset { get; set; }
            public ushort delay_num { get; set; }
            public ushort delay_den { get; set; }
            public byte dispose_op { get; set; }
            public byte blend_op { get; set; }

            public fcTL(byte[] array)
            {
                validateData(array, 38, "fcTL");
                sequence_number = EngineBase.getSwappedUInt(array, 8);
                width = EngineBase.getSwappedUInt(array, 12);
                height = EngineBase.getSwappedUInt(array, 16);
                x_offset = EngineBase.getSwappedUInt(array, 20);
                y_offset = EngineBase.getSwappedUInt(array, 24);
                delay_num = EngineBase.getSwappedUShort(array, 28);
                delay_den = EngineBase.getSwappedUShort(array, 30);
                dispose_op = array[32];
                blend_op = array[33];
            }

            public fcTL(
                IHDR ihdr,
                uint x_offset,
                uint y_offset,
                ushort delay_num,
                ushort delay_den,
                byte dispose_op,
                byte blend_op
                )
            {
                width = ihdr.width;
                height = ihdr.height;
                this.x_offset = x_offset;
                this.y_offset = y_offset;
                this.delay_num = delay_num;
                this.delay_den = delay_den;
                this.dispose_op = dispose_op;
                this.blend_op = blend_op;
            }

            public override byte[] ToByteArray()
            {
                var array = new byte[38];
                EngineBase.getSwappedArray(26).CopyTo(array, 0);
                Encoding.Default.GetBytes("fcTL").CopyTo(array, 4);
                EngineBase.getSwappedArray((int)sequence_number).CopyTo(array, 8);
                EngineBase.getSwappedArray((int)width).CopyTo(array, 12);
                EngineBase.getSwappedArray((int)height).CopyTo(array, 16);
                EngineBase.getSwappedArray((int)x_offset).CopyTo(array, 20);
                EngineBase.getSwappedArray((int)y_offset).CopyTo(array, 24);
                EngineBase.getSwappedArray((short)delay_num).CopyTo(array, 28);
                EngineBase.getSwappedArray((short)delay_den).CopyTo(array, 30);
                EngineBase.getSwappedArray(dispose_op).CopyTo(array, 32);
                EngineBase.getSwappedArray(blend_op).CopyTo(array, 33);
                EngineBase.getSwappedCrc(array.Skip(4).Take(26).ToArray()).CopyTo(array, 34);
                return array;
            }
        }

        internal class IHDR : Chunk
        {
            public uint width { get; set; }
            public uint height { get; set; }
            public byte bit_depth { get; set; }
            public byte color_type { get; set; }
            public byte compression_method { get; set; }
            public byte filter_method { get; set; }
            public byte interlace_method { get; set; }

            public IHDR(byte[] array)
            {
                validateData(array, 25, "IHDR");
                width = EngineBase.getSwappedUInt(array, 8);
                height = EngineBase.getSwappedUInt(array, 12);
                bit_depth = array[16];
                color_type = array[17];
                compression_method = array[18];
                filter_method = array[19];
                interlace_method = array[20];
            }

            public IHDR(
                fcTL fctl,
                byte bit_depth,
                byte color_type,
                byte compression_method,
                byte filter_method,
                byte interlace_method
                )
            {
                width = fctl.width;
                height = fctl.height;
                this.bit_depth = bit_depth;
                this.color_type = color_type;
                this.compression_method = compression_method;
                this.filter_method = filter_method;
                this.interlace_method = interlace_method;
            }

            public override byte[] ToByteArray()
            {
                var array = new byte[25];
                EngineBase.getSwappedArray(13).CopyTo(array, 0);
                Encoding.Default.GetBytes("IHDR").CopyTo(array, 4);
                EngineBase.getSwappedArray((int)width).CopyTo(array, 8);
                EngineBase.getSwappedArray((int)height).CopyTo(array, 12);
                EngineBase.getSwappedArray(bit_depth).CopyTo(array, 16);
                EngineBase.getSwappedArray(color_type).CopyTo(array, 17);
                EngineBase.getSwappedArray(compression_method).CopyTo(array, 18);
                EngineBase.getSwappedArray(filter_method).CopyTo(array, 19);
                EngineBase.getSwappedArray(interlace_method).CopyTo(array, 20);
                EngineBase.getSwappedCrc(array.Skip(4).Take(17).ToArray()).CopyTo(array, 21);
                return array;
            }
        }

        internal class IDAT : Chunk
        {
            public uint Length { get; set; }
            public byte[] Data { get; set; }

            public IDAT(byte[] array)
            {
                if (array.Length < 12)
                    throw new Exception("Invalid IDAT array length. It has to be at least 12 bytes.");
                Length = EngineBase.getSwappedUInt(array, 0);
                validateData(array, (int)Length + 12, "IDAT");
                Data = new byte[Length];
                Array.Copy(array, 8, Data, 0, Length);
            }

            public override byte[] ToByteArray()
            {
                var array = new byte[Length + 12];
                EngineBase.getSwappedArray((int)Length).CopyTo(array, 0);
                Encoding.Default.GetBytes("IDAT").CopyTo(array, 4);
                Array.Copy(Data, 0, array, 8, Length);
                EngineBase.skipLengthPropertyAndGetSwappedCrc(array).CopyTo(array, Length + 8);
                return array;
            }
        }

        internal class fdAT : Chunk
        {
            public uint sequence_number { get; set; }
            public uint Length { get; set; }
            public byte[] Data { get; set; }

            public fdAT(byte[] array)
            {
                if (array.Length < 12)
                    throw new Exception("Invalid fdAT array length. It has to be at least 12 bytes.");
                Length = EngineBase.getSwappedUInt(array, 0);
                validateData(array, (int)Length + 12, "fdAT");
                sequence_number = EngineBase.getSwappedUInt(array, 8);
                Data = new byte[Length - 4];
                Array.Copy(array, 12, Data, 0, Length - 4);
            }

            public override byte[] ToByteArray()
            {
                var array = new byte[Length + 12];
                EngineBase.getSwappedArray((int)Length).CopyTo(array, 0);
                Encoding.Default.GetBytes("fdAT").CopyTo(array, 4);
                EngineBase.getSwappedArray((int)sequence_number).CopyTo(array, 8);
                Array.Copy(Data, 0, array, 12, Length - 4);
                EngineBase.skipLengthPropertyAndGetSwappedCrc(array).CopyTo(array, Length + 12);
                return array;
            }
        }

        public string FilePath { get; set; }
        public List<Frame> Frames { get; set; }
    }
}
