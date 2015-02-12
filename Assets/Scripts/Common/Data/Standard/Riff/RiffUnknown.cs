using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffUnknown : RiffChunk
	{
		public readonly Byte[] dataArray;

		public RiffUnknown( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			Logger.Debug( "Unknown ID:" + aId + ", Size:" + aSize );

			dataArray = aByteArray.ReadBytes( ( int )Size );
		}
	}
}
