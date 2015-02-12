using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public abstract class MusicPcm : IMusic
	{
		private List<List<LoopInformation>> loopList;
		
		public string Name{ get; private set; }
		public SoundTime Length{ get; private set; }
		public WaveformReaderPcm Waveform{ get; private set; }
		public LoopInformation Loop{ get; set; }

		public int GetCountLoopX()
		{
			return loopList.Count;
		}
		
		public int GetCountLoopY( int aIndexX )
		{
			if( aIndexX < GetCountLoopX() )
			{
				return loopList[aIndexX].Count;
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
				return loopList[aIndexX][aIndexY];
			}
			else
			{
				return new LoopInformation( 44100, -1, -1 );
			}
		}

		public MusicPcm( FormAiffForm aFormFile )
		{
			Name = aFormFile.name;
			Waveform = new WaveformReaderPcm( aFormFile, false );
			Length = new SoundTime( Waveform.format.sampleRate, Waveform.format.samples );

			loopList = new List<List<LoopInformation>>();
			loopList.Add( new List<LoopInformation>() );
			loopList[0].Add( new LoopInformation( Length.sampleRate, -1, -1 ) );
		}
		
		public MusicPcm( RiffWaveRiff aRiffFile )
		{
			Name = aRiffFile.name;
			Waveform = new WaveformReaderPcm( aRiffFile, false );
			
			Length = new SoundTime( Waveform.format.sampleRate, Waveform.format.samples );

			RiffWaveSmpl lRiffWaveSmpl = ( RiffWaveSmpl )aRiffFile.GetChunk( RiffWaveSmpl.ID );
			
			if( lRiffWaveSmpl != null )
			{
				loopList = new List<List<LoopInformation>>();
				
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
						loopList.Add( new List<LoopInformation>() );
						lLoopLength = ( int )( lLoop.end - lLoop.start );
						lIndex++;
					}
					
					loopList[lIndex].Add( new LoopInformation( Length.sampleRate, ( int )lLoop.start, ( int )lLoop.end ) );
				}
			}
			else
			{
				loopList = new List<List<LoopInformation>>();
				loopList.Add( new List<LoopInformation>() );
				loopList[0].Add( new LoopInformation( Length.sampleRate, -1, -1 ) );
			}

			Loop = GetLoop( 0, 0 );
		}
	}
}
