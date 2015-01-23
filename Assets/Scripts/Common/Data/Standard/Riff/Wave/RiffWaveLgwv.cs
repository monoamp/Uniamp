using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveLgwv : RiffChunk
	{
		public const string ID = "LGWV";

		public readonly byte[] dataArray;

		public RiffWaveLgwv( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			dataArray = aByteArray.ReadBytes( ( int )Size );
		}
	}
}
