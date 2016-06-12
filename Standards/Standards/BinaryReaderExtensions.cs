using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standards
{
	public static class BinaryReaderExtensions
	{
		public static Int32 ReadStringInt32(this BinaryReader binaryReader, int length)
		{
			return Int32.Parse(binaryReader.ReadString(length));
		}

		public static Double ReadStringLatitude(this BinaryReader binaryReader)
		{
			int degrees = binaryReader.ReadStringInt32(3);
			int minutes = binaryReader.ReadStringInt32(2);
			int seconds = binaryReader.ReadStringInt32(2);
			char hemisphere = binaryReader.ReadChar();
			if (hemisphere == 'S')
			{
				degrees = -degrees;
			}
			double latitude = degrees + (minutes / 60.0) + (seconds / 3600);
			return latitude;
		}

		public static Double ReadStringLongitude(this BinaryReader binaryReader)
		{
			int degrees = binaryReader.ReadStringInt32(3);
			int minutes = binaryReader.ReadStringInt32(2);
			int seconds = binaryReader.ReadStringInt32(2);
			char hemisphere = binaryReader.ReadChar();
			if (hemisphere == 'W')
			{
				degrees = -degrees;
			}
			double longitude = degrees + (minutes / 60.0) + (seconds / 3600);
			return longitude;
		}

		public static UInt16 ReadUInt16BigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToUInt16(binaryReader.ReadBytesReversed(sizeof(UInt16)), 0);
		}

		public static Int16 ReadInt16BigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToInt16(binaryReader.ReadBytesReversed(sizeof(Int16)), 0);
		}

		public static UInt32 ReadUInt32BigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToUInt32(binaryReader.ReadBytesReversed(sizeof(UInt32)), 0);
		}

		public static Int32 ReadInt32BigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToInt32(binaryReader.ReadBytesReversed(sizeof(Int32)), 0);
		}

		public static UInt64 ReadUInt64BigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToUInt64(binaryReader.ReadBytesReversed(sizeof(UInt64)), 0);
		}

		public static Int64 ReadInt64BigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToInt64(binaryReader.ReadBytesReversed(sizeof(Int64)), 0);
		}

		public static Single ReadSingleBigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToSingle(binaryReader.ReadBytesReversed(sizeof(Single)), 0);
		}

		public static Double ReadDoubleBigEndian(this BinaryReader binaryReader)
		{
			return BitConverter.ToDouble(binaryReader.ReadBytesReversed(sizeof(Double)), 0);
		}

		public static String ReadString(this BinaryReader binaryReader, int length)
		{
			return new String(binaryReader.ReadChars(length));
		}

		private static byte[] ReadBytesReversed(this BinaryReader binaryReader, int count)
		{
			return binaryReader.ReadBytes(sizeof(UInt16)).Reverse().ToArray();
		}
	}
}
