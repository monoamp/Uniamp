using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveDisp : RiffChunk
	{
		public const string ID = "DISP";

		public readonly UInt32 type;
		public readonly UInt32 data;

		public RiffWaveDisp( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			type = aByteArray.ReadUInt32();
			data = aByteArray.ReadUInt32();
		}
	}
}
