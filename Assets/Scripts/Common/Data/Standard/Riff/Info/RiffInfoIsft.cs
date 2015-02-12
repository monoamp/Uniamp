using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIsft : RiffChunk
	{
		public const string ID = "ISFT";

		public readonly string software;

		public RiffInfoIsft( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			software = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Software:" + software );
			Logger.BreakDebug( "Software:" + software );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( software );
		}*/
	}
}
