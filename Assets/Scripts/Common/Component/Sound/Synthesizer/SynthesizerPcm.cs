using System;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Component.Sound.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.Synthesizer
{
	public class SynthesizerPcm
	{
		private MusicPcm music;
		private SoundTime position;
		private SoundTime positionPre;
		private SoundTime elapsed;

		public LoopInformation Loop{ get; private set; }
		public int loopNumberX;
		public int loopNumberY;

		public bool isLoop;

		public SynthesizerPcm( MusicPcm aMusicPcm )
		{
			music = aMusicPcm;
			position = new SoundTime( 44100, 0 );
			positionPre = new SoundTime( 44100, 0 );
			elapsed = new SoundTime( 44100, 0 );
			
			Loop = music.GetLoop( loopNumberX, loopNumberY );
			loopNumberX = 0;
			loopNumberY = 0;

			isLoop = false;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			if( isLoop == true )
			{
				if( Loop.length.sample > 0 )
				{
					if( positionPre.sample <= Loop.end.sample + 1 && position.sample >= Loop.end.sample + 1 )
					{
						Logger.Debug( "Loop " + position.sample + " to " + ( Loop.start.sample + positionPre.sample - ( Loop.end.sample + 1 ) ) );

						position.sample = Loop.start.sample + positionPre.sample - ( Loop.end.sample + 1 );
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
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, position.sample, Loop.start.sample );
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

		public int GetLoopNumberX()
		{
			return loopNumberX;
		}

		public int GetLoopNumberY()
		{
			return loopNumberY;
		}

		public void SetNextLoop()
		{
			loopNumberX++;
			loopNumberX %= music.GetCountLoopX();
			loopNumberY = 0;
			Loop = music.GetLoop( loopNumberX, loopNumberY );
		}

		public void SetPreviousLoop()
		{
			loopNumberX += music.GetCountLoopX();
			loopNumberX--;
			loopNumberX %= music.GetCountLoopX();
			loopNumberY = 0;
			Loop = music.GetLoop( loopNumberX, loopNumberY );
		}

		public void SetUpLoop()
		{
			loopNumberY++;
			loopNumberY %= music.GetCountLoopY( loopNumberX );
			Loop = music.GetLoop( loopNumberX, loopNumberY );
		}

		public void SetDownLoop()
		{
			loopNumberY += music.GetCountLoopY( loopNumberX );
			loopNumberY--;
			loopNumberY %= music.GetCountLoopY( loopNumberX );
			Loop = music.GetLoop( loopNumberX, loopNumberY );
		}
	}
}
