using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveRiff : RiffChunkList
	{
		public const string ID = "RIFF";
		
		public static readonly Dictionary<string, Type> chunkTypeDictionaryWave;
		public static readonly Dictionary<string, Dictionary<string,Type>> chunkTypeDictionaryDictionary;
		public override Dictionary<string, Dictionary<string,Type>> ChunkTypeDictionaryDictionary{ get{ return chunkTypeDictionaryDictionary; } }

		public readonly string name;

		static RiffWaveRiff()
		{
			chunkTypeDictionaryWave = new Dictionary<string, Type>();
			chunkTypeDictionaryWave.Add( RiffWaveList.ID, typeof( RiffWaveList ) );
			chunkTypeDictionaryWave.Add( RiffWaveBext.ID, typeof( RiffWaveBext ) );
			chunkTypeDictionaryWave.Add( RiffWaveCue_.ID, typeof( RiffWaveCue_ ) );
			chunkTypeDictionaryWave.Add( RiffWaveData.ID, typeof( RiffWaveData ) );
			chunkTypeDictionaryWave.Add( RiffWaveDisp.ID, typeof( RiffWaveDisp ) );
			chunkTypeDictionaryWave.Add( RiffWaveFact.ID, typeof( RiffWaveFact ) );
			chunkTypeDictionaryWave.Add( RiffWaveFile.ID, typeof( RiffWaveFile ) );
			chunkTypeDictionaryWave.Add( RiffWaveFmt_.ID, typeof( RiffWaveFmt_ ) );
			chunkTypeDictionaryWave.Add( RiffWaveInst.ID, typeof( RiffWaveInst ) );
			chunkTypeDictionaryWave.Add( RiffWaveLabl.ID, typeof( RiffWaveLabl ) );
			chunkTypeDictionaryWave.Add( RiffWaveLgwv.ID, typeof( RiffWaveLgwv ) );
			chunkTypeDictionaryWave.Add( RiffWaveLtxt.ID, typeof( RiffWaveLtxt ) );
			chunkTypeDictionaryWave.Add( RiffWaveNote.ID, typeof( RiffWaveNote ) );
			chunkTypeDictionaryWave.Add( RiffWavePlst.ID, typeof( RiffWavePlst ) );
			chunkTypeDictionaryWave.Add( RiffWaveSmpl.ID, typeof( RiffWaveSmpl ) );
			
			chunkTypeDictionaryDictionary = new Dictionary<string, Dictionary<string,Type>>();
			chunkTypeDictionaryDictionary.Add( "WAVE", chunkTypeDictionaryWave );
		}
		
		public RiffWaveRiff( string aPathFile )
			: this( new FileStream( aPathFile, FileMode.Open, FileAccess.Read ) )
		{
			
		}

		public RiffWaveRiff( FileStream aStream )
			: this( new ByteArrayLittle( aStream ) )
		{

		}

		public RiffWaveRiff( ByteArray aByteArray )
			: base( aByteArray.ReadString( 4 ), aByteArray.ReadUInt32(), aByteArray, null )
		{
			name = aByteArray.GetName();
		}
		
		public RiffWaveRiff( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
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

					foreach( RiffChunk lChunk in chunkList )
					{
						lChunk.WriteByteArray( lByteArray, aByteArray );
					}
				}
			}
		}
	}
}
