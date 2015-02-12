using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveLtxt : RiffChunk
	{
		public const string ID = "ltxt";

		public readonly UInt32 name;
		public readonly UInt32 sampleLength;
		public readonly UInt32 purpose;
		public readonly UInt16 country;
		public readonly UInt16 language;
		public readonly UInt16 dialect;
		public readonly UInt16 codePage;
		public readonly Byte[] data;

		public RiffWaveLtxt( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			name = aByteArray.ReadUInt32();
			sampleLength = aByteArray.ReadUInt32();
			purpose = aByteArray.ReadUInt32();
			country = aByteArray.ReadUInt16();
			language = aByteArray.ReadUInt16();
			dialect = aByteArray.ReadUInt16();
			codePage = aByteArray.ReadUInt16();
			data = aByteArray.ReadBytes( Size - 20 );

			informationList.Add( "    Name:" + name );
			informationList.Add( "    Sample Length:" + sampleLength );
			informationList.Add( "    Purpose:" + purpose );
			informationList.Add( "    Country:" + country );
			informationList.Add( "    Language:" + language );
			informationList.Add( "    Dialect:" + dialect );
			informationList.Add( "    CodePage:" + codePage );
			informationList.Add( "    Data:" + data.Length );
		}
	}
}
