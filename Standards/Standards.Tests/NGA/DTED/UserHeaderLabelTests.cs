using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using Standards.NGA.DTED;

namespace Standards.Tests.NGA.DTED
{
	[TestClass]
	public class UserHeaderLabelTests
	{
		[TestMethod]
		public void RoundTrip()
		{
			Type type = typeof(UserHeaderLabel);
			object obj1 = Activator.CreateInstance(type);
			string str1 = BinarySerializer.SerializeToString(obj1);
			object obj2 = BinarySerializer.Deserialize(type, str1);
			string str2 = BinarySerializer.SerializeToString(obj2);
			Console.WriteLine("'{0}'", str1);
			Console.WriteLine("'{0}'", str2);
			Assert.AreEqual(str1, str2);
		}

		[TestMethod]
		public void LengthCorrect()
		{
			Type type = typeof(UserHeaderLabel);
			object obj1 = Activator.CreateInstance(type);
			string str1 = BinarySerializer.SerializeToString(obj1);
			Assert.AreEqual(80 , str1.Length);
		}
	}
}
