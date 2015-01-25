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

		private SoundTime position;
		private SoundTime positionPre;
		private SoundTime elapsed;

		public LoopInformation loop;
		public bool isLoop;

		public SynthesizerPcm( MusicPcm aMusicPcm )
		{
			music = aMusicPcm;

			position = new SoundTime( 44100, 0 );
			positionPre = new SoundTime( 44100, 0 );
			elapsed = new SoundTime( 44100, 0 );
			
			loop = music.GetLoop( 0, 0 );
			isLoop = false;
		}
		
		public SynthesizerPcm( WaveformWave aWaveform, LoopInformation aLoop )
		{
			waveform = aWaveform;

			position = new SoundTime( 44100, 0 );
			positionPre = new SoundTime( 44100, 0 );
			elapsed = new SoundTime( 44100, 0 );
			
			loop = aLoop;
			isLoop = false;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			if( isLoop == true )
			{
				if( loop.length.sample > 0 )
				{
					if( positionPre.sample <= loop.end.sample + 1 && position.sample >= loop.end.sample + 1 )
					{
						Logger.Debug( "Loop " + position.sample + " to " + ( loop.start.sample + positionPre.sample - ( loop.end.sample + 1 ) ) );

						position.sample = loop.start.sample + positionPre.sample - ( loop.end.sample + 1 );
					}
				}
				else
				{
					if( position.sample >= music.Length.sample )
					{
						Logger.Debug( "Loop " + position.sample + " to " + ( positionPre.sample - music.Length.sample ) );

						position.sample = positionPre.sample - music.Length.sample;
					}
				}
			}

			if( position.sample + 1 < music.Length.sample )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, position.sample );
				}
			}
			else if( position.sample < music.Length.sample )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, position.sample, loop.start.sample );
				}
			}

			positionPre.sample = position.sample;
			position.sample += ( double )music.Length.sampleRate / ( double )aSampleRate;
			elapsed.sample += ( double )music.Length.sampleRate / ( double )aSampleRate;
		}

		public void SetPosition( double aPosition )
		{
			positionPre.sample = music.Length.sample * aPosition;
			position.sample = music.Length.sample * aPosition;
		}

		public double GetPosition()
		{
			return position.sample / music.Length.sample;
		}

		public SoundTime GetTimePosition()
		{
			return position;
		}

		public SoundTime GetTimeElapsed()
		{
			return elapsed;
		}

		public SoundTime GetSoundTime()
		{
			return music.Length;
		}
	}
}
