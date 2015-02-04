using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public class RiffInfoIsng : RiffChunk
	{
		public const string ID = "isng";

		public readonly string unknown;

		public RiffInfoIsng( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			unknown = aByteArray.ReadString( ( int )Size );

			informationList.Add( "Unknown:" + unknown );
		}
		/*
		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			aByteArray.WriteString( unknown );
		}*/
	}
}
