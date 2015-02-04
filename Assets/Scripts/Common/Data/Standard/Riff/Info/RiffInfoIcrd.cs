using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIcrd : RiffChunk
	{
		public const string ID = "ICRD";

		public readonly string creationDate;

		public RiffInfoIcrd( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			creationDate = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Creation Date:" + creationDate );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( creationDate );
		}*/
	}
}
