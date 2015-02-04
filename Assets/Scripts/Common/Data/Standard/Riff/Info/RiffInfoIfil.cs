using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIfil : RiffChunk
	{
		public const string ID = "ifil";

		public readonly string unknown;

		public RiffInfoIfil( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			unknown = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Unknown:" + unknown );
			Logger.BreakDebug( "Unknown:" + unknown );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( unknown );
		}*/
	}
}
