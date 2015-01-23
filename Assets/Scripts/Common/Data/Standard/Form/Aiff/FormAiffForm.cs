using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Form.Aiff
{
	public class FormAiffForm : FormChunkList
	{
		public const string ID = "FORM";
		
		public static readonly Dictionary<string, Type> chunkTypeDictionaryAiff;
		public static readonly Dictionary<string, Type> chunkTypeDictionaryAifc;
		public static readonly Dictionary<string, Dictionary<string,Type>> chunkTypeDictionaryDictionary;
		public override Dictionary<string, Dictionary<string,Type>> ChunkTypeDictionaryDictionary{ get{ return chunkTypeDictionaryDictionary; } }

		public readonly string name;

		static FormAiffForm()
		{
			chunkTypeDictionaryAiff = new Dictionary<string, Type>();
			chunkTypeDictionaryAiff.Add( FormAiffChan.ID, typeof( FormAiffChan ) );
			chunkTypeDictionaryAiff.Add( FormAiffComm.ID, typeof( FormAiffComm ) );
			chunkTypeDictionaryAiff.Add( FormAiffComt.ID, typeof( FormAiffComt ) );
			chunkTypeDictionaryAiff.Add( FormAiffMark.ID, typeof( FormAiffMark ) );
			chunkTypeDictionaryAiff.Add( FormAiffSsnd.ID, typeof( FormAiffSsnd ) );
			
			chunkTypeDictionaryAifc = new Dictionary<string, Type>();
			chunkTypeDictionaryAifc.Add( FormAiffChan.ID, typeof( FormAiffChan ) );
			chunkTypeDictionaryAifc.Add( FormAiffComm.ID, typeof( FormAiffComm ) );
			chunkTypeDictionaryAifc.Add( FormAiffComt.ID, typeof( FormAiffComt ) );
			chunkTypeDictionaryAifc.Add( FormAiffMark.ID, typeof( FormAiffMark ) );
			chunkTypeDictionaryAifc.Add( FormAiffSsnd.ID, typeof( FormAiffSsnd ) );

			chunkTypeDictionaryDictionary = new Dictionary<string, Dictionary<string,Type>>();
			chunkTypeDictionaryDictionary.Add( "AIFF", chunkTypeDictionaryAiff );
			chunkTypeDictionaryDictionary.Add( "AIFC", chunkTypeDictionaryAifc );
		}
		
		public FormAiffForm( string aPathFile )
			: this( new FileStream( aPathFile, FileMode.Open, FileAccess.Read ) )
		{
			
		}

		public FormAiffForm( FileStream aStream )
			: this( new ByteArrayBig( aStream ) )
		{

		}

		public FormAiffForm( ByteArray aByteArray )
			: base( aByteArray.ReadString( 4 ), aByteArray.ReadUInt32(), aByteArray, null )
		{
			name = aByteArray.GetName();
		}
		
		public FormAiffForm( string aId, UInt32 aSize, ByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			if( name != null && name != "" )
			{
				using ( FileStream u = new FileStream( name, FileMode.Open, FileAccess.Read ) )
				{
					ByteArray lByteArray = new ByteArrayLittle( u );

					foreach( FormChunk lChunk in chunkList )
					{
						lChunk.WriteByteArray( lByteArray, aByteArray );
					}
				}
			}
		}
	}
}
