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
		private SoundTime timeElapsed;

		public int loopNumber1;
		public int loopNumber2;

		public bool isLoop;

		public SynthesizerPcm( MusicPcm aMusicPcm )
		{
			music = aMusicPcm;
			timePosition = new SoundTime( 44100, 0 );
			timeElapsed = new SoundTime( 44100, 0 );
			loopNumber1 = 0;
			loopNumber2 = 0;
			isLoop = false;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			LoopInformation lLoop = music.Loop[loopNumber1][loopNumber2];

			if( isLoop == true && lLoop.length.sample > 0 && ( int )timePosition.sample > lLoop.end.sample )
			{
				Logger.Debug( "Start:" + lLoop.start.sample + ", End:" + lLoop.end.sample );

				double diff = timePosition.sample % 1.0d;
				timePosition.sample = ( int )lLoop.start.sample + diff;
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
			else
			{
				timePosition.sample = 0.0d;
			}

			if( timePosition.sample == 1 )
			{
				Logger.Debug( "Start:" + aSoundBuffer[0] );
			}

			timePosition.sample += ( double )music.Sample.sampleRate / ( double )aSampleRate;
			timeElapsed.sample += ( double )music.Sample.sampleRate / ( double )aSampleRate;
		}

		public void SetPosition( double aPosition )
		{
			timePosition.sample = ( double )music.Sample.sample * aPosition;
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
