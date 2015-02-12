using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIcmt : RiffChunk
	{
		public const string ID = "ICMT";

		public readonly string comment;

		public RiffInfoIcmt( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			comment = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Comment:" + comment );
			Logger.BreakDebug( "Comment:" + comment );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( comment );
		}*/
	}
}
