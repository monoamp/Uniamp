using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIsbj : RiffChunk
	{
		public const string ID = "ISBJ";

		public readonly string subject;

		public RiffInfoIsbj( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			subject = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Subject:" + subject );
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( subject );
		}
	}
}
