using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveNote : RiffChunk
	{
		public const string ID = "note";

		public readonly UInt32 name;
		public readonly string data;

		public RiffWaveNote( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			name = aByteArray.ReadUInt32();
			data = aByteArray.ReadString( ( int )Size - 4 );

			informationList.Add( "    Name:" + name );
			informationList.Add( "    Data:" + data );
		}
	}
}
