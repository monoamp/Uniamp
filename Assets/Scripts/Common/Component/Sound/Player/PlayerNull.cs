using System;
using System.Collections.Generic;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.Player
{
	public class PlayerNull : IPlayer
    {
		public double PositionRate{ get{ return 0.0d; } set{} }
		public float Volume{ get; set; }
		public bool IsMute{ get; set; }
		public bool IsLoop{ get; set; }
		
		public LoopInformation Loop{ get{ return new LoopInformation( 44100, -1, -1 ); } }
		public int LoopNumberX{ get{ return 0; } }
		public int LoopNumberY{ get{ return 0; } }

        public PlayerNull()
		{
			Volume = 0.5f;
			IsMute = false;
			IsLoop = true;
		}

		public void Play()
		{

		}

		public void Stop()
		{

		}

		public void Pause()
		{

		}

		public void Record( string aPath )
		{

		}

		public bool GetFlagPlaying()
		{
            return false;
		}
		
		public string GetFilePath()
		{
			return "";
		}

        public SoundTime GetTPosition()
        {
            return new SoundTime( 44100, 0 );
        }
        
        public SoundTime GetElapsed()
        {
            return new SoundTime( 44100, 0 );
        }
        
        public SoundTime GetLength()
        {
            return new SoundTime( 44100, 0 );
        }

		public int Update( float[] aSoundBuffer, int aChannels, int aSampleRate, int aPositionInBuffer )
		{
			return aSoundBuffer.Length / aChannels;
		}

		public void SetPreviousLoop()
		{

		}

		public void SetNextLoop()
		{

		}

		public void SetUpLoop()
		{

		}

		public void SetDownLoop()
		{

		}
	}
}
