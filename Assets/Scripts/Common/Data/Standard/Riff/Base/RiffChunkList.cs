using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff
{
	public abstract class RiffChunkList : RiffChunk
	{
		public abstract Dictionary<string, Dictionary<string, Type>> ChunkTypeDictionaryDictionary{ get; }

		public readonly List<RiffChunk> chunkList;
		public readonly Dictionary<string, List<RiffChunkList>> listListDictionary;

		public string type;
		
		public override UInt32 Size
		{
			get{ return GetSize(); }
			protected set{ base.Size = value; }
		}
		
		private UInt32 GetSize()
		{
			UInt32 _size = 4;
			
			foreach( RiffChunk lChunk in chunkList )
			{
				_size += lChunk.Size + 8;
			}
			
			return _size;
		}

		protected RiffChunkList( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			chunkList = new List<RiffChunk>();
			listListDictionary = new Dictionary<string, List<RiffChunkList>>();
			
			type = aByteArray.ReadString( 4 );

			try
			{
				while( aByteArray.Position < position + aSize - 4 )
				{
					ReadChunk( aByteArray );
				}
			}
			catch( Exception aExpection )
			{
				Logger.Error( "Expection at RIFF Read:" + aExpection.ToString() );
			}
			
			aByteArray.SetPosition( ( int )( position + aSize ) );
		}

		private void ReadChunk( ByteArray aByteArray )
		{
			string lId = aByteArray.ReadString( 4 );

			UInt32 lSize = aByteArray.ReadUInt32();

			int lPositionStart = aByteArray.Position;
			
			// Check Padding.
			if( lSize % 2 == 1 )
			{
				if( lPositionStart + lSize <= aByteArray.Length && aByteArray.ReadByte( ( int )( lPositionStart + lSize ) ) == 0x00 )
				{
					lSize++;

					Logger.Debug( "Padding:" + lSize );
				}

				aByteArray.SetPosition( lPositionStart );
			}

			RiffChunk lRiffWave = Construct( lId, lSize, aByteArray, this );

			chunkList.Add( lRiffWave );

			if( aByteArray.Position != lPositionStart + lSize )
			{
				Logger.Debug( "Modify Position:" + aByteArray.Position + "->" + ( lPositionStart + lSize ) );
				aByteArray.SetPosition( ( int )( lPositionStart + lSize ) );
			}
		}

		public RiffChunk Construct( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
		{
			Type[] lArgumentTypes = { typeof( string ), typeof( UInt32 ), typeof( ByteArray ), typeof( RiffChunkList ) };
			object[] lArguments = { aId, aSize, aByteArray, aParent };

			Type lTypeChunk = typeof( RiffUnknown );
			
			if( ChunkTypeDictionaryDictionary.ContainsKey( type ) == true )
			{
				if( ChunkTypeDictionaryDictionary[type].ContainsKey( aId ) == true )
				{
					lTypeChunk = ChunkTypeDictionaryDictionary[type][aId];
				}
			}
			
			if( lTypeChunk == typeof( RiffUnknown ) )
			{
				Logger.Debug( "Unknown:" + type + "," + aId );
			}
			else
			{
				Logger.Debug( "known:" + type + "," + aId );
			}

			return ( RiffChunk )lTypeChunk.GetConstructor( lArgumentTypes ).Invoke( lArguments );
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			for( int i = 0; i < id.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )id[i] );
			}

			aByteArray.WriteUInt32( ( UInt32 )Size );

			for( int i = 0; i < type.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )type[i] );
			}

			foreach( RiffChunk lChunk in chunkList )
			{
				lChunk.WriteByteArray( aByteArrayRead, aByteArray );
			}
		}

		protected void OverrideChunk( RiffChunk aChunk )
		{
			for( int i = 0; i < chunkList.Count; i++ )
			{
				if( chunkList[i].id == aChunk.id )
				{
					chunkList[i] = aChunk;

					return;
				}
			}
		}

		public RiffChunk GetChunk( string aId )
		{
			for( int i = 0; i < chunkList.Count; i++ )
			{
				if( chunkList[i].id == aId )
				{
					return chunkList[i];
				}
			}

			return null;
		}

		protected List<RiffChunkList> GetChunkListList( string aId, string aType )
		{
			if( listListDictionary.ContainsKey( aType ) == true ) {
				return listListDictionary[aType];
			}

			List<RiffChunkList> lListList = new List<RiffChunkList>();

			for( int i = 0; i < chunkList.Count; i++ )
			{
				if( chunkList[i].id == aId) {
					RiffChunkList lRiffList = ( RiffChunkList )chunkList[i];

					if( lRiffList.type == aType ) {
						lListList.Add( ( RiffChunkList )chunkList[i] );
					}
				}
			}

			listListDictionary.Add( aType, lListList );

			return listListDictionary[aType];
		}

		protected RiffChunkList GetChunkList( string aId, string aType )
		{
			if( listListDictionary.ContainsKey( aType ) == true )
			{
				return listListDictionary[aType][0];
			}

			List<RiffChunkList> lListList = new List<RiffChunkList>();

			for( int i = 0; i < chunkList.Count; i++ )
			{
				if( chunkList[i].id == aId )
				{
					RiffChunkList lRiffList = ( RiffChunkList )chunkList[i];

					if( lRiffList.type == aType )
					{
						lListList.Add( ( RiffChunkList )chunkList[i] );
					}
				}
			}

			listListDictionary.Add( aType, lListList );

			if( listListDictionary[aType].Count < 1 )
			{
				Logger.Error( "List is not exist." );
			}
			else if( listListDictionary[aType].Count > 1 )
			{
				Logger.Error( "List exist more than 1." );
			}
			
			return listListDictionary[aType][0];
		}

		protected void AddChunk( RiffChunk aRiffWave )
		{
			chunkList.Add( aRiffWave );
		}
	}
}
