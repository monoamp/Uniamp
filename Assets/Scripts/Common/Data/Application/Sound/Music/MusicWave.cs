using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.system.io;
using Monoamp.Common.Struct;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Music
{
	public class MusicWave : MusicPcm
	{
		public MusicWave( string aPathFile )
			: this( PoolCollection.GetWav( aPathFile ) )
		{
			
		}

		public MusicWave( RiffWaveRiff aRiffFile )
		{
			nameFile = aRiffFile.name;
			
			RiffWaveData lRiffWaveData = ( RiffWaveData )aRiffFile.GetChunk( RiffWaveData.ID );
			bytePosition = ( int )lRiffWaveData.position;
			byteSize = ( int )lRiffWaveData.Size;

			RiffWaveFmt_ lRiffWaveFmt_ = ( RiffWaveFmt_ )aRiffFile.GetChunk( RiffWaveFmt_.ID );
			Channels = lRiffWaveFmt_.channels;
			SampleBits = lRiffWaveFmt_.bitsPerSample;
			Sample = new SoundTime( ( int )lRiffWaveFmt_.samplesPerSec, ( int )( byteSize / ( SampleBits / 8 ) / Channels ) );

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
			
			RiffWaveSmpl lRiffWaveSmpl = ( RiffWaveSmpl )aRiffFile.GetChunk( RiffWaveSmpl.ID );

			if( lRiffWaveSmpl != null )
			{
				Loop = new List<List<LoopInformation>>();

				int lIndex = -1;
				int lLoopLength = -1;

				for( int i = 0; i < lRiffWaveSmpl.sampleLoops; i++ )
				{
					SampleLoop lLoop = lRiffWaveSmpl.sampleLoopList[i];

					if( ( int )( lLoop.end - lLoop.start ) == lLoopLength )
					{
						
					}
					else
					{
						Loop.Add( new List<LoopInformation>() );
						lLoopLength = ( int )( lLoop.end - lLoop.start );
						lIndex++;
					}

					Loop[lIndex].Add( new LoopInformation( Sample.sampleRate, ( int )lLoop.start, ( int )lLoop.end ) );
				}
			}
			else
			{
				Loop = new List<List<LoopInformation>>();
				Loop.Add( new List<LoopInformation>() );
				Loop[0].Add( new LoopInformation( Sample.sampleRate, -1, -1 ) );
			}
		}
		
		protected override ByteArray ConstructByteArray( FileStream aFileStream )
		{
			return new ByteArrayLittle( aFileStream );
		}
	}
}
