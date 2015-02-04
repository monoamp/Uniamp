using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIart : RiffChunk
	{
		public const string ID = "IART";

		public readonly string artist;

		public RiffInfoIart( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			artist = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Artist:" + artist );
			Logger.BreakDebug( "Artist:" + artist );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( artist );
		}*/
	}
}
