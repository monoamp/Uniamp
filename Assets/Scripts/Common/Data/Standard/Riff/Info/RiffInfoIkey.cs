using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIkey : RiffChunk
	{
		public const string ID = "IKEY";

		public readonly string keywords;

		public RiffInfoIkey( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			keywords = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Keywords:" + keywords );
			Logger.BreakDebug( "Keywords:" + keywords );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( keywords );
		}*/
	}
}
