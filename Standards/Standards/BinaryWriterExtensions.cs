using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standards
{
	public static class BinaryWriterExtensions
	{
		public static void WriteStringInt(this BinaryWriter binaryWriter, int value, int length, char leftFillChar)
		{
			string valueStr = value.ToString().PadLeft(length, leftFillChar);
			binaryWriter.Write(valueStr, length);
		}

		public static void WriteStringLeftJustified(this BinaryWriter binaryWriter, string str, int length, char fillChar)
		{
			string valueStr = "";
			if (str != null)
			{
				valueStr = str.PadLeft(length, fillChar);
			}
			binaryWriter.Write(valueStr, length);
		}

		public static void WriteStringRightJustified(this BinaryWriter binaryWriter, string str, int length, char fillChar)
		{
			string valueStr = "";
			if (str != null)
			{
				valueStr = str.PadRight(length, fillChar);
			}
			binaryWriter.Write(valueStr, length);
		}

		public static void WriteStringLatitude(this BinaryWriter binaryWriter, double latitude)
		{
			int degrees = (int)Math.Floor(latitude);
			double minutesDouble = (latitude - degrees) * 60.0;
			int minutes = (int)Math.Floor(minutesDouble);
			double secondsDouble = (minutesDouble - minutes) * 60.0;
			int seconds = (int)Math.Round(secondsDouble, 0);
			char hemisphere = 'N';
			if (degrees < 0)
			{
				hemisphere = 'S';
				degrees = -degrees;
			}
			binaryWriter.WriteStringInt(degrees, 3, '0');
			binaryWriter.WriteStringInt(minutes, 2, '0');
			binaryWriter.WriteStringInt(degrees, 2, '0');
			binaryWriter.Write(hemisphere);
		}

		public static void WriteStringLongitude(this BinaryWriter binaryWriter, double longitude)
		{
			int degrees = (int)Math.Floor(longitude);
			double minutesDouble = (longitude - degrees) * 60.0;
			int minutes = (int)Math.Floor(minutesDouble);
			double secondsDouble = (minutesDouble - minutes) * 60.0;
			int seconds = (int)Math.Round(secondsDouble, 0);
			char hemisphere = 'E';
			if (degrees < 0)
			{
				hemisphere = 'W';
				degrees = -degrees;
			}
			binaryWriter.WriteStringInt(degrees, 3, '0');
			binaryWriter.WriteStringInt(minutes, 2, '0');
			binaryWriter.WriteStringInt(degrees, 2, '0');
			binaryWriter.Write(hemisphere);
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, Int16 value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, UInt16 value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, Int32 value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, UInt32 value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, Int64 value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, UInt64 value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, float value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void WriteBigEndian(this BinaryWriter binaryWriter, double value)
		{
			binaryWriter.ReverseWrite(BitConverter.GetBytes(value));
		}

		public static void Write(this BinaryWriter binaryWriter, string value, int length)
		{
			int slice = 0;
			if (value != null && value.Length <= length)
			{
				slice = value.Length;
			}
			for (int i = 0; i < slice; i++)
			{
				binaryWriter.Write(value[i]);
			}
			for (int i = slice; i < length; i++)
			{
				binaryWriter.Write(' ');
			}
		}

		private static void ReverseWrite(this BinaryWriter binaryWriter, byte[] bytes)
		{
			for (int i = bytes.Length - 1; i >= 0; i--)
			{
				binaryWriter.Write(bytes[i]);
			}
		}
	}
}
