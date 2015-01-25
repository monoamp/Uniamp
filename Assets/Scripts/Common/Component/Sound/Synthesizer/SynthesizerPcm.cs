using System;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Data.Application.Waveform;
using Monoamp.Common.Component.Sound.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.Synthesizer
{
	public class SynthesizerPcm
	{
		private readonly MusicPcm music;
		private readonly WaveformWave waveform;

		public SoundTime Position{ get; private set; }
		public SoundTime PositionPre{ get; private set; }
		public SoundTime Elapsed{ get; private set; }

		public LoopInformation loop;
		public bool isLoop;

		public double PositionRate
		{
			get { return Position.sample / music.Length.sample; }
			set { PositionPre.sample = Position.sample = music.Length.sample * value; }
		}

		public SynthesizerPcm( MusicPcm aMusicPcm )
		{
			music = aMusicPcm;

			Position = new SoundTime( 44100, 0 );
			PositionPre = new SoundTime( 44100, 0 );
			Elapsed = new SoundTime( 44100, 0 );
			
			loop = music.GetLoop( 0, 0 );
			isLoop = false;
		}
		
		public SynthesizerPcm( WaveformWave aWaveform, LoopInformation aLoop )
		{
			waveform = aWaveform;

			Position = new SoundTime( 44100, 0 );
			PositionPre = new SoundTime( 44100, 0 );
			Elapsed = new SoundTime( 44100, 0 );
			
			loop = aLoop;
			isLoop = false;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			if( isLoop == true )
			{
				if( loop.length.sample > 0 )
				{
					if( PositionPre.sample <= loop.end.sample + 1 && Position.sample >= loop.end.sample + 1 )
					{
						Logger.Debug( "Loop " + Position.sample + " to " + ( loop.start.sample + PositionPre.sample - ( loop.end.sample + 1 ) ) );

						Position.sample = loop.start.sample + PositionPre.sample - ( loop.end.sample + 1 );
					}
				}
				else
				{
					if( Position.sample >= music.Length.sample )
					{
						Logger.Debug( "Loop " + Position.sample + " to " + ( PositionPre.sample - music.Length.sample ) );

						Position.sample = PositionPre.sample - music.Length.sample;
					}
				}
			}

			if( Position.sample + 1 < music.Length.sample )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, Position.sample );
				}
			}
			else if( Position.sample < music.Length.sample )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, Position.sample, loop.start.sample );
				}
			}

			PositionPre.sample = Position.sample;
			Position.sample += ( double )music.Length.sampleRate / ( double )aSampleRate;
			Elapsed.sample += ( double )music.Length.sampleRate / ( double )aSampleRate;
		}
	}
}
