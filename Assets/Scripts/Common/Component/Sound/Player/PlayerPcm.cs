using System;
using System.Collections.Generic;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Component.Sound.Synthesizer;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.Player
{
	public class PlayerPcm : IPlayer
	{
		public double PositionRate{ get{ return synthesizer.PositionRate; } set{ synthesizer.PositionRate = value; } }
		public float Volume{ get; set; }
		public bool IsMute{ get; set; }
		public bool IsLoop{ get{ return synthesizer.isLoop; } set{ synthesizer.isLoop = value; } }

		public LoopInformation Loop{ get { return synthesizer.loop; } private set{ synthesizer.loop = value; } }
		public int LoopNumberX{ get; private set; }
		public int LoopNumberY{ get; private set; }

		private SynthesizerPcm synthesizer;
		private MusicPcm music;

		private delegate void DelegateUpdate( float[] aSoundBuffer, int aChannels, int aSampleRate );
		private DelegateUpdate delegateUpdate;

		//private string path;

        private float[] bufferArray;

		/*
		public PlayerPcm()
		{
			Volume = 0.5f;
			IsMute = false;
			IsLoop = true;
		}
		*/

		public PlayerPcm( string aFilePath )
			: this( ( MusicPcm )LoaderCollection.LoadMusic( aFilePath ) )
		{

		}

		public PlayerPcm( MusicPcm aMusic )
		{
			music = aMusic;
			synthesizer = new SynthesizerPcm( music.waveform, music.GetLoop( 0, 0 ) );
            bufferArray = new float[2];

			delegateUpdate = UpdatePlay;

			Volume = 0.5f;
			IsMute = false;
			IsLoop = true;
		}

		public void Play()
		{
			delegateUpdate = UpdatePlay;
		}

		public void Stop()
		{
			delegateUpdate = UpdateSynth;
		}

		public void Pause()
		{
			if( delegateUpdate == UpdatePlay )
			{
				delegateUpdate = UpdateSynth;
			}
			else
			{
				delegateUpdate = UpdatePlay;
			}
		}

		public void Record( string aPath )
		{
			//path = aPath;

			delegateUpdate = UpdateRecord;
		}

		public bool GetFlagPlaying()
		{
			if( delegateUpdate == UpdatePlay )
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public SoundTime GetTPosition()
		{
			return synthesizer.Position;
		}

		public SoundTime GetElapsed()
		{
			return synthesizer.Elapsed;
		}

		public SoundTime GetLength()
		{
			return music.Length;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			delegateUpdate( aSoundBuffer, aChannels, aSampleRate );
		}

		public void UpdatePlay( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			int lLength = aSoundBuffer.Length / aChannels;

			for( int i = 0; i < lLength; i++ )
			{
				synthesizer.Update( bufferArray, aChannels, aSampleRate );

				if( IsMute == false )
				{
					for( int j = 0; j < aChannels; j++ )
					{
						aSoundBuffer[i * aChannels + j] = bufferArray[j] * Volume;
					}
				}
			}
		}

		public void UpdateRecord( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			delegateUpdate = UpdateSynth;
		}

		public void UpdateSynth( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			for( int i = 0; i < aSoundBuffer.Length; i++ )
			{
				aSoundBuffer[i] = 0.0f;
			}
		}

		public int GetLoopNumberX()
		{
			return LoopNumberX;
		}
		
		public int GetLoopNumberY()
		{
			return LoopNumberY;
		}
		
		public void SetNextLoop()
		{
			LoopNumberX++;
			LoopNumberX %= music.GetCountLoopX();
			LoopNumberY = 0;
			Loop = music.GetLoop( LoopNumberX, LoopNumberY );
		}
		
		public void SetPreviousLoop()
		{
			LoopNumberX += music.GetCountLoopX();
			LoopNumberX--;
			LoopNumberX %= music.GetCountLoopX();
			LoopNumberY = 0;
			Loop = music.GetLoop( LoopNumberX, LoopNumberY );
		}
		
		public void SetUpLoop()
		{
			LoopNumberY++;
			LoopNumberY %= music.GetCountLoopY( LoopNumberX );
			Loop = music.GetLoop( LoopNumberX, LoopNumberY );
		}
		
		public void SetDownLoop()
		{
			LoopNumberY += music.GetCountLoopY( LoopNumberX );
			LoopNumberY--;
			LoopNumberY %= music.GetCountLoopY( LoopNumberX );
			Loop = music.GetLoop( LoopNumberX, LoopNumberY );
		}
	}
}
