using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIcop : RiffChunk
	{
		public const string ID = "ICOP";

		public readonly string corporation;

		public RiffInfoIcop( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			corporation = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Corporation:" + corporation );
			Logger.BreakDebug( "Corporation:" + corporation );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( corporation );
		}*/
	}
}
