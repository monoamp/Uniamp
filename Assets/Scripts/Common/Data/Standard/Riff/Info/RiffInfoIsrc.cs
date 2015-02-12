using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIsrc : RiffChunk
	{
		public const string ID = "ISRC";

		public readonly string source;

		public RiffInfoIsrc( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			source = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Source:" + source );
			Logger.BreakDebug( "Source:" + source );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( source );
		}*/
	}
}
