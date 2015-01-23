﻿using System;
using System.Collections.Generic;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.Player
{
	public class PlayerNull : IPlayer
    {
		public double Position{ get{ return 0.0d; } set{} }
		public float Volume{ get; set; }
		public bool IsLoop{ get{ return false; } set{} }

        public PlayerNull()
		{

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
        
        public SoundTime GetTimePosition()
        {
            return new SoundTime( 44100, 0 );
        }
        
        public SoundTime GetTimeElapsed()
        {
            return new SoundTime( 44100, 0 );
        }
        
        public SoundTime GetTimeLength()
        {
            return new SoundTime( 44100, 0 );
        }

		public LoopInformation GetLoopPoint()
		{
            return null;
		}

		public int GetLoopCount()
		{
            return 0;
		}

		public int GetLoopNumberX()
        {
            return 0;
		}

		public int GetLoopNumberY()
        {
            return 0;
		}

		public void Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

		}

		public void UpdatePlay( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

		}

		public void UpdateRecord( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

		}

		public void UpdateSynth( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

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
