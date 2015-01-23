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
		private SynthesizerPcm synthesizer;

		private delegate void DelegateUpdate( float[] aSoundBuffer, int aChannels, int aSampleRate );
		private DelegateUpdate delegateUpdate;

		//private string path;

        private float[] bufferArray;

        public double Position{ get{ return synthesizer.GetPosition(); } set{ synthesizer.SetPosition( value ); } }
		public float Volume{ get; set; }
		public bool IsLoop{ get{ return synthesizer.isLoop; } set{ synthesizer.isLoop = value; } }

		public PlayerPcm( string aFilePath )
			: this( ( MusicPcm )LoaderCollection.LoadMusic( aFilePath ) )
		{

		}

		public PlayerPcm( MusicPcm aMusic )
		{
			synthesizer = new SynthesizerPcm( aMusic );
            bufferArray = new float[2];

			delegateUpdate = UpdatePlay;

			Volume = 0.5f;
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

		public SoundTime GetTimePosition()
		{
			return synthesizer.GetTimePosition ();
		}

		public SoundTime GetTimeElapsed()
		{
			return synthesizer.GetTimeElapsed ();
		}

		public SoundTime GetTimeLength()
		{
			return synthesizer.GetSoundTime ();
		}

		public LoopInformation GetLoopPoint()
		{
			return synthesizer.GetLoopPoint();
		}

		public int GetLoopCount()
		{
			return synthesizer.GetLoopCount();
		}

		public int GetLoopNumberX()
		{
			return synthesizer.GetLoopNumberX();
		}

		public int GetLoopNumberY()
		{
			return synthesizer.GetLoopNumberY();
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

				for( int j = 0; j < aChannels; j++ )
				{
					aSoundBuffer[i * aChannels + j] = bufferArray[j] * Volume;
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

		public void SetPreviousLoop()
		{
			synthesizer.SetPreviousLoop();
		}

		public void SetNextLoop()
		{
			synthesizer.SetNextLoop();
		}

		public void SetUpLoop()
		{
			synthesizer.SetUpLoop();
		}

		public void SetDownLoop()
		{
			synthesizer.SetDownLoop();
		}
	}
}
