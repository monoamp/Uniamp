using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Data.Application.Waveform;
using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public struct Int24
	{
		public const int MinValue = -8388608;
		public const int MaxValue = 8388607;
	}

	public abstract class MusicPcm : IMusic
	{
		public WaveformPcm waveform;

		protected const int LENGTH_BUFFER = 1024 * 4;

		public SoundTime Length{ get{ return waveform.format.length; } }

		protected List<List<LoopInformation>> LoopList{ get; set; }
		protected string nameFile{ get; set; }

		public int GetCountLoopX()
		{
			return LoopList.Count;
		}
		
		public int GetCountLoopY( int aIndexX )
		{
			if( aIndexX < GetCountLoopX() )
			{
				return LoopList[aIndexX].Count;
			}
			else
			{
				return 0;
			}
		}
		
		public LoopInformation GetLoop( int aIndexX, int aIndexY )
		{
			if( aIndexX < GetCountLoopX() && aIndexY < GetCountLoopY( aIndexX ) )
			{
				return LoopList[aIndexX][aIndexX];
			}
			else
			{
				return new LoopInformation( 44100, -1, -1 );
			}
		}

		public MusicPcm( FormAiffForm aFormFile )
		{
			nameFile = aFormFile.name;
			waveform = new WaveformPcm( aFormFile );
			
			LoopList = new List<List<LoopInformation>>();
			LoopList.Add( new List<LoopInformation>() );
			LoopList[0].Add( new LoopInformation( Length.sampleRate, -1, -1 ) );
		}
		
		public MusicPcm( RiffWaveRiff aRiffFile )
		{
			nameFile = aRiffFile.name;
			waveform = new WaveformPcm( aRiffFile );
			
			RiffWaveSmpl lRiffWaveSmpl = ( RiffWaveSmpl )aRiffFile.GetChunk( RiffWaveSmpl.ID );
			
			if( lRiffWaveSmpl != null )
			{
				LoopList = new List<List<LoopInformation>>();
				
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
						LoopList.Add( new List<LoopInformation>() );
						lLoopLength = ( int )( lLoop.end - lLoop.start );
						lIndex++;
					}
					
					LoopList[lIndex].Add( new LoopInformation( Length.sampleRate, ( int )lLoop.start, ( int )lLoop.end ) );
				}
			}
			else
			{
				LoopList = new List<List<LoopInformation>>();
				LoopList.Add( new List<LoopInformation>() );
				LoopList[0].Add( new LoopInformation( Length.sampleRate, -1, -1 ) );
			}
		}
	}
}
