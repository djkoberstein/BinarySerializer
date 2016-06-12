using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Standards
{
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
