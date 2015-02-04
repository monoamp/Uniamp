using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIeng : RiffChunk
	{
		public const string ID = "IENG";

		public readonly string engineer;

		public RiffInfoIeng( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			engineer = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Engineer:" + engineer );
			Logger.BreakDebug( "Engineer:" + engineer );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( engineer );
		}*/
	}
}
