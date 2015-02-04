using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIgnr : RiffChunk
	{
		public const string ID = "IGNR";

		public readonly string genre;

		public RiffInfoIgnr( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			genre = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Genre:" + genre );
			Logger.BreakDebug( "Genre:" + genre );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( genre );
		}*/
	}
}
