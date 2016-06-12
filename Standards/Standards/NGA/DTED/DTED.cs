using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Standards.NGA.DTED
{
	public class UserHeaderLabel
	{
		public string RecognitionSentinel { get; set; }
		public char FixedByStandard { get; set; }
		public double LongitudeOfOrigin { get; set; }
		public double LatitiudeOfOrigin { get; set; }
		public double LongitudeInterval { get; set; }
		public double LatitudeInterval { get; set; }
		public string AbsoluteVerticalAccuracy { get; set; }
		public string SecurityCode { get; set; }
		public string UniqueReference { get; set; }
		public int NumberOfLongitudeLines { get; set; }
		public int NumberOfLatitudePoints { get; set; }
		public char MultipleAccuracy { get; set; }
		public string Reserved { get; set; }

		public UserHeaderLabel()
		{
			RecognitionSentinel = "UHL";
			FixedByStandard = '1';
			MultipleAccuracy = ' ';
		}

		public static UserHeaderLabel Deserialize(BinaryReader binaryReader)
		{
			UserHeaderLabel uhl = new UserHeaderLabel();
			uhl.RecognitionSentinel = binaryReader.ReadString(3);
			uhl.FixedByStandard = binaryReader.ReadChar();
			uhl.LatitiudeOfOrigin = binaryReader.ReadStringLatitude();
			uhl.LongitudeOfOrigin = binaryReader.ReadStringLongitude();
			uhl.LongitudeInterval = binaryReader.ReadStringInt32(4) / 360.0;
			uhl.LatitudeInterval = binaryReader.ReadStringInt32(4) / 360.0;
			uhl.AbsoluteVerticalAccuracy = binaryReader.ReadString(4);
			uhl.SecurityCode = binaryReader.ReadString(3);
			uhl.UniqueReference = binaryReader.ReadString(12);
			uhl.NumberOfLongitudeLines = binaryReader.ReadStringInt32(4);
			uhl.NumberOfLatitudePoints = binaryReader.ReadStringInt32(4);
			uhl.MultipleAccuracy = binaryReader.ReadChar();
			uhl.Reserved = binaryReader.ReadString(24);
			return uhl;
		}

		public static void Serialize (BinaryWriter binaryWriter, UserHeaderLabel uhl)
		{
			binaryWriter.Write(uhl.RecognitionSentinel, 3);
			binaryWriter.Write(uhl.FixedByStandard);
			binaryWriter.WriteStringLatitude(uhl.LatitiudeOfOrigin);
			binaryWriter.WriteStringLongitude(uhl.LongitudeOfOrigin);
			binaryWriter.WriteStringInt((int)(uhl.LongitudeInterval * 360.0), 4, '0');
			binaryWriter.WriteStringInt((int)(uhl.LatitudeInterval * 360.0), 4, '0');
			binaryWriter.WriteStringRightJustified(uhl.AbsoluteVerticalAccuracy, 4, '0');
			binaryWriter.WriteStringLeftJustified(uhl.SecurityCode, 3, ' ');
			binaryWriter.WriteStringLeftJustified(uhl.UniqueReference, 12, ' ');
			binaryWriter.WriteStringInt(uhl.NumberOfLongitudeLines, 4, '0');
			binaryWriter.WriteStringInt(uhl.NumberOfLatitudePoints, 4, '0');
			binaryWriter.Write(uhl.MultipleAccuracy);
			binaryWriter.Write(uhl.Reserved, 24);
		}
	}

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

	public static class BinarySerializer
	{
		public static string SerializeToString(object obj)
		{
			byte[] bytes = SerializeToBytes(obj);
			return Encoding.UTF8.GetString(bytes);
		}

		public static byte[] SerializeToBytes(object obj)
		{
			using (var memoryStream = new MemoryStream())
			{
				Serialize(obj, memoryStream);
				return memoryStream.ToArray();
			}
		}

		public static void Serialize(object obj, Stream stream)
		{
			using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				Serialize(obj, binaryWriter);
			}
		}

		public static void Serialize(object obj, BinaryWriter binaryWriter)
		{
			Type type = obj.GetType();
			MethodInfo methodInfo = type.GetMethod("Serialize");
			if (methodInfo == null)
			{
				throw new InvalidOperationException("static Serialize method is required for type " + type);
			}
			methodInfo.Invoke(null, new object[] { binaryWriter, obj });
		}

		public static object Deserialize(Type type, string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			object result = Deserialize(type, bytes);
			return result;
		}

		public static object Deserialize(Type type, byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				object result = Deserialize(type, memoryStream);
				return result;
			}
		}

		public static object Deserialize(Type type, Stream stream)
		{
			using (var binaryReader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				object result = Deserialize(type, binaryReader);
				return result;
			}
		}

		public static object Deserialize(Type type, BinaryReader binaryReader)
		{
			MethodInfo methodInfo = type.GetMethod("Deserialize");
			if (methodInfo == null)
			{
				throw new InvalidOperationException("static Deserialize method is required for type " + type);
			}
			object result = methodInfo.Invoke(null, new object[] { binaryReader });
			return result;
		}
	}
}
