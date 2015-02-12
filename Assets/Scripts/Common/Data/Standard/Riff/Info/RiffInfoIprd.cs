using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIprd : RiffChunk
	{
		public const string ID = "IPRD";

		public readonly string product;

		public RiffInfoIprd( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			product = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Product:" + product );
			Logger.BreakDebug( "Product:" + product );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( product );
		}*/
	}
}
