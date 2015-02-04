using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoInam : RiffChunk
	{
		public const string ID = "INAM";

		public readonly string name;

		public RiffInfoInam( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			name = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Name:" + name );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( name );
		}*/
	}
}
