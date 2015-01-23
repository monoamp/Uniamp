using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Form
{
	public abstract class FormChunk
	{
		public readonly string id;
		public readonly FormChunkList parent;
		public readonly UInt32 position;
		public readonly List<string> informationList;
		public virtual UInt32 Size{ get; protected set; }

		protected FormChunk( string aId, UInt32 aSize, ByteArray aByteArray, FormChunkList aParent )
		{
			id = aId;
			Size = aSize;
			parent = aParent;

			if( aByteArray != null )
			{
				position = ( UInt32 )aByteArray.Position;
			}

			informationList = new List<string>();
		}

		public virtual void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			for( int i = 0; i < id.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )id[i] );
			}

			aByteArray.WriteUInt32( ( UInt32 )Size );
			aByteArrayRead.SetPosition( ( int )position );
			aByteArray.WriteBytes( aByteArrayRead.ReadBytes( Size ) );
		}
	}
}
