using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Struct;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Music
{
	public class MusicAiff : MusicPcm
	{
		public MusicAiff( string aPathFile )
			: this( PoolCollection.GetAif( aPathFile ) )
		{

		}

		public MusicAiff( FormAiffForm aFormFile )
		{
			nameFile = aFormFile.name;

			FormAiffSsnd lSsndChunk = ( FormAiffSsnd )aFormFile.GetChunk( FormAiffSsnd.ID );
			bytePosition = ( int )( lSsndChunk.position + lSsndChunk.offset + 8 );
			byteSize = lSsndChunk.dataSize;

			FormAiffComm lChunkComm = ( FormAiffComm )aFormFile.GetChunk( FormAiffComm.ID );
			Channels = lChunkComm.numberOfChannels;
			SampleBits = lChunkComm.bitsPerSamples;
			Sample = new SoundTime( ( int )lChunkComm.sampleRate, ( int )( byteSize / ( SampleBits / 8 ) / Channels ) );

			LengthBuffer = ( int )Sample.sample;

			if( LENGTH_BUFFER != 0 )
			{
				LengthBuffer = LENGTH_BUFFER;
			}
			
			SampleArray = new float[Channels][];

			for( int i = 0; i < Channels; i++ )
			{
				SampleArray[i] = new float[LengthBuffer];
			}
			
			StartPosition = int.MaxValue;

			Loop = new List<List<LoopInformation>>();
			Loop.Add( new List<LoopInformation>() );
			Loop[0].Add( new LoopInformation( Sample.sampleRate, -1, -1 ) );
		}
		
		protected override ByteArray ConstructByteArray( FileStream aFileStream )
		{
			return new ByteArrayBig( aFileStream );
		}
	}
}
