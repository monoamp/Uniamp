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
		private SoundTime timePosition;
		private SoundTime timePositionPre;
		private SoundTime timeElapsed;

		public int loopNumber1;
		public int loopNumber2;

		public bool isLoop;

		public SynthesizerPcm( MusicPcm aMusicPcm )
		{
			music = aMusicPcm;
			timePosition = new SoundTime( 44100, 0 );
			timePositionPre = new SoundTime( 44100, 0 );
			timeElapsed = new SoundTime( 44100, 0 );
			loopNumber1 = 0;
			loopNumber2 = 0;
			isLoop = false;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			LoopInformation lLoop = music.Loop[loopNumber1][loopNumber2];

			if( isLoop == true )
			{
				if( lLoop.length.sample > 0 )
				{
					if( timePositionPre.sample <= lLoop.end.sample + 1 && timePosition.sample >= lLoop.end.sample + 1 )
					{
						Logger.Debug( "Loop " + timePosition.sample + " to " + ( lLoop.start.sample + timePositionPre.sample - ( lLoop.end.sample + 1 ) ) );

						timePosition.sample = lLoop.start.sample + timePositionPre.sample - ( lLoop.end.sample + 1 );
					}
				}
				else
				{
					if( timePosition.sample >= music.Sample.sample )
					{
						Logger.Debug( "Loop " + timePosition.sample + " to " + ( timePositionPre.sample - music.Sample.sample ) );

						timePosition.sample = timePositionPre.sample - music.Sample.sample;
					}
				}
			}

			if( timePosition.sample + 1 < music.Sample.sample )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, timePosition.sample );
				}
			}
			else if( timePosition.sample < music.Sample.sample )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( music, i, timePosition.sample, ( int )lLoop.start.sample );
				}
			}

			timePositionPre.sample = timePosition.sample;
			timePosition.sample += ( double )music.Sample.sampleRate / ( double )aSampleRate;
			timeElapsed.sample += ( double )music.Sample.sampleRate / ( double )aSampleRate;
		}

		public void SetPosition( double aPosition )
		{
			timePositionPre.sample = music.Sample.sample * aPosition;
			timePosition.sample = music.Sample.sample * aPosition;
		}

		public double GetPosition()
		{
			return timePosition.sample / music.Sample.sample;
		}

		public SoundTime GetTimePosition()
		{
			return timePosition;
		}

		public SoundTime GetTimeElapsed()
		{
			return timeElapsed;
		}

		public SoundTime GetSoundTime()
		{
			return music.Sample;
		}

		public LoopInformation GetLoopPoint()
		{
			return music.Loop[loopNumber1][loopNumber2];
		}

		public int GetLoopCount()
		{
			return music.Loop[loopNumber1].Count;
		}

		public int GetLoopNumberX()
		{
			return loopNumber1;
		}

		public int GetLoopNumberY()
		{
			return loopNumber2;
		}

		public void SetNextLoop()
		{
			loopNumber1++;
			loopNumber1 %= music.Loop.Count;

			loopNumber2 = 0;
		}

		public void SetPreviousLoop()
		{
			loopNumber1 += music.Loop.Count;
			loopNumber1--;
			loopNumber1 %= music.Loop.Count;

			loopNumber2 = 0;
		}

		public void SetUpLoop()
		{
			loopNumber2++;
			loopNumber2 %= music.Loop[loopNumber1].Count;
		}

		public void SetDownLoop()
		{
			loopNumber2 += music.Loop[loopNumber1].Count;
			loopNumber2--;
			loopNumber2 %= music.Loop[loopNumber1].Count;
		}
	}
}
