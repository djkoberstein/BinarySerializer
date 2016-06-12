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
}
