using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIcms : RiffChunk
	{
		public const string ID = "ICMS";

		public readonly string commisioned;

		public RiffInfoIcms( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			commisioned = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Commisioned:" + commisioned );
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( commisioned );
		}
	}
}
