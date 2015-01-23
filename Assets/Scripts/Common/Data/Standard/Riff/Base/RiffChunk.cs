using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff
{
	public abstract class RiffChunk
	{
		public readonly string id;
		public readonly RiffChunkList parent;
		public readonly UInt32 position;
		public readonly List<string> informationList;
		
		public virtual UInt32 Size{ get; protected set; }

		protected RiffChunk( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
		{
			id = aId;
			Size = aSize;
			parent = aParent;

			if( aByteArray != null )
			{
				position = ( UInt32 )aByteArray.Position;
			}

			informationList = new List<string>();
			
			Monoamp.Boundary.Logger.Debug( "Position:" + position.ToString() );

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
