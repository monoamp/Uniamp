using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveFact : RiffChunk
	{
		public const string ID = "fact";

		public readonly UInt32 sampleLength;

		public RiffWaveFact( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			sampleLength = aByteArray.ReadUInt32();

			informationList.Add( "Sample Length:" + sampleLength );
		}
	}
}
