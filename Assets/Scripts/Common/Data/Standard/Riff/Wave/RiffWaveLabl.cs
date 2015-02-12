using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveLabl : RiffChunk
	{
		public const string ID = "labl";

		public readonly UInt32 name;
		public readonly string data;

		public RiffWaveLabl( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			name = aByteArray.ReadUInt32();
			data = aByteArray.ReadString( ( int )Size - 4 );
			
			Logger.BreakDebug( "    Name:" + name );
			Logger.BreakDebug( "    Data:" + data );

			informationList.Add( "    Name:" + name );
			informationList.Add( "    Data:" + data );
		}
	}
}
