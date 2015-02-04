using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoItch : RiffChunk
	{
		public const string ID = "ITCH";

		public readonly string technician;

		public RiffInfoItch( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			technician = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Technician:" + technician );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( technician );
		}*/
	}
}
