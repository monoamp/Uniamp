using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Form
{
	public abstract class FormChunkList : FormChunk
	{
		public abstract Dictionary<string, Dictionary<string, Type>> ChunkTypeDictionaryDictionary{ get; }

		public readonly List<FormChunk> chunkList;
		public readonly Dictionary<string, List<FormChunkList>> listListDictionary;

		public string type;

		public override UInt32 Size
		{
			get{ return GetSize(); }
			protected set{ base.Size = value;}
		}

		private UInt32 GetSize()
		{
			UInt32 _size = 4;
			
			foreach( FormChunk lChunk in chunkList )
			{
				_size += lChunk.Size + 8;
			}
			
			return _size;
		}

		protected FormChunkList( string aId, UInt32 aSize, ByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			chunkList = new List<FormChunk>();
			listListDictionary = new Dictionary<string, List<FormChunkList>>();

			type = aByteArray.ReadString( 4 );

			try
			{
				while( aByteArray.Position < position + aSize )
				{
					ReadChunk( aByteArray );
				}
			}
			catch( Exception aExpection )
			{
				Logger.Error( "Expection at Form Read:" + aExpection.ToString() );
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
				else
				{
					Logger.Debug( "No Padding:" + lSize );
				}

				aByteArray.SetPosition( lPositionStart );
			}

			FormChunk lRiffWave = Construct( lId, lSize, aByteArray, this );

			chunkList.Add( lRiffWave );
			
			if( aByteArray.Position != lPositionStart + lSize )
			{
				Logger.Debug( "Modify Position:" + aByteArray.Position + "->" + ( lPositionStart + lSize ) );
				aByteArray.SetPosition( ( int )( lPositionStart + lSize ) );
			}
		}

		public FormChunk Construct( string aId, UInt32 aSize, ByteArray aByteArray, FormChunkList aParent )
		{
			Type[] lArgumentTypes = { typeof( string ), typeof( UInt32 ), typeof( ByteArray ), typeof( FormChunkList ) };
			object[] lArguments = { aId, aSize, aByteArray, aParent };

			Type lTypeChunk = typeof( FormUnknown );

			if( ChunkTypeDictionaryDictionary.ContainsKey( type ) == true )
			{
				if( ChunkTypeDictionaryDictionary[type].ContainsKey( aId ) == true )
				{
					lTypeChunk = ChunkTypeDictionaryDictionary[type][aId];
				}
			}

			if( lTypeChunk == typeof( FormUnknown ) )
			{
				Logger.Debug( "Unknown:" + type + "," + aId );
			}
			else
			{
				Logger.Debug( "known:" + type + "," + aId );
			}

			return ( FormChunk )lTypeChunk.GetConstructor( lArgumentTypes ).Invoke( lArguments );
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			for( int i = 0; i < id.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )id[i] );
			}

			aByteArray.WriteUInt32( Size );

			for( int i = 0; i < type.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )type[i] );
			}

			foreach( FormChunk lChunk in chunkList )
			{
				lChunk.WriteByteArray( aByteArrayRead, aByteArray );
			}
		}

		protected void OverrideChunk( FormChunk aChunk )
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

		public FormChunk GetChunk( string aId )
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

		protected List<FormChunkList> GetChunkListList( string aId, string aType )
		{
			if( listListDictionary.ContainsKey( aType ) == true )
			{
				return listListDictionary[aType];
			}

			List<FormChunkList> lListList = new List<FormChunkList>();

			for( int i = 0; i < chunkList.Count; i++ )
			{
				if( chunkList[i].id == aId )
				{
					FormChunkList lRiffList = ( FormChunkList )chunkList[i];

					if( lRiffList.type == aType )
					{
						lListList.Add( ( FormChunkList )chunkList[i] );
					}
				}
			}

			listListDictionary.Add( aType, lListList );

			return listListDictionary[aType];
		}

		protected FormChunkList GetChunkList( string aId, string aType )
		{
			if( listListDictionary.ContainsKey( aType ) == true )
			{
				return listListDictionary[aType][0];
			}

			List<FormChunkList> lListList = new List<FormChunkList>();

			for( int i = 0; i < chunkList.Count; i++ )
			{
				if( chunkList[i].id == aId )
				{
					FormChunkList lRiffList = ( FormChunkList )chunkList[i];

					if( lRiffList.type == aType )
					{
						lListList.Add( ( FormChunkList )chunkList[i] );
					}
				}
			}

			listListDictionary.Add( aType, lListList );

			if( listListDictionary[aType].Count < 1 )
			{
				Logger.Error( "List is not exist.");
			}
			else if( listListDictionary[aType].Count > 1 )
			{
				Logger.Error( "List exist lather than 1.");
			}

			return listListDictionary[aType][0];
		}

		protected void AddChunk( FormChunk aRiffWave )
		{
			chunkList.Add( aRiffWave );
		}
	}
}
