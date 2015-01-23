using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Form.Aiff
{
	public class FormAiffSsnd : FormChunk
	{
		public const string ID = "SSND";

		public readonly UInt32 offset;
		public readonly UInt32 blockSize;
		public readonly string comment;

		public readonly int dataSize;

		public FormAiffSsnd( string aId, UInt32 aSize, ByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			offset = aByteArray.ReadUInt32();
			blockSize = aByteArray.ReadUInt32();
			comment = aByteArray.ReadString( ( int )offset );

			dataSize = ( int )( Size - offset - 8 );
			aByteArray.AddPosition( dataSize );

			informationList.Add( "Offset:" + offset );
			informationList.Add( "Block Size:" + blockSize );
			informationList.Add( "Comment:" + comment );
		}
	}
}
