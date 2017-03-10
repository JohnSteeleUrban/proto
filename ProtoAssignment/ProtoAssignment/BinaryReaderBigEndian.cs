using System;
using System.IO;
using System.Text;

namespace ProtoAssignment
{
   public class BinaryReaderBigEndian : BinaryReader
    {
        public BinaryReaderBigEndian(Stream input) : base(input)
        {
        }

        public BinaryReaderBigEndian(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public BinaryReaderBigEndian(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override byte[] ReadBytes(int count)
        {
            var reverse = base.ReadBytes(count);
            Array.Reverse(reverse);
            return reverse;
        }

        public UInt32 ReadConvertToUInt32(int i, int i1)
        {
            return BitConverter.ToUInt32(ReadBytes(i), i1);
        }

        public Mps7.RecordType ConvertRecType(int i)
        {
            return (Mps7.RecordType)Int32.Parse(BitConverter.ToString(ReadBytes(i)));
        }

        public ulong ReadConvertToUInt64(int i, int i1)
        {
            return BitConverter.ToUInt64(ReadBytes(i), i1);
        }

        public double ReadDollars(int i, int i1)
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }
    }
}
