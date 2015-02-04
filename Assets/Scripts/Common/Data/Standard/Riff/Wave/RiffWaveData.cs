using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveData : RiffChunk
	{
		public const string ID = "data";

		public RiffWaveData( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			aByteArray.AddPosition( ( int )Size );
		}
	}
}
