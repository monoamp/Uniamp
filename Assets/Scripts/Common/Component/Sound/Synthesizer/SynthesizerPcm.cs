using System;
using System.Collections.Generic;

using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.Component.Sound.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.Synthesizer
{
	public class SynthesizerPcm
	{
		private readonly WaveformReaderPcm waveform;
		
		public bool isLoop;
		public LoopInformation loop;
		
		public Dictionary<int, SoundTime> oneSampelList;
		public SoundTime Position{ get; private set; }
		public SoundTime PositionPre{ get; private set; }
		public SoundTime Elapsed{ get; private set; }

		public double PositionRate
		{
			get { return Position.sample / waveform.format.samples; }
			set { PositionPre = Position = new SoundTime( PositionPre.sampleRate, waveform.format.samples * value ); }
		}

		public SynthesizerPcm( WaveformReaderPcm aWaveform, LoopInformation aLoop )
		{
			waveform = aWaveform;
			
			oneSampelList = new Dictionary<int, SoundTime>();
			Position = new SoundTime( waveform.format.sampleRate, 0 );
			PositionPre = new SoundTime( waveform.format.sampleRate, 0 );
			Elapsed = new SoundTime( waveform.format.sampleRate, 0 );
			
			loop = aLoop;
			isLoop = false;
		}

		// Return: Ture if end.
		public bool Update( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			if( isLoop == true )
			{
				if( loop.length.sample > 0 )
				{
					if( PositionPre.sample <= loop.end.sample + 1 && Position.sample >= loop.end.sample + 1 )
					{
						Logger.Debug( "Loop " + Position.sample + " to " + ( loop.start.sample + PositionPre.sample - ( loop.end.sample + 1 ) ) );
						
						//Position.sample = loop.start.sample + PositionPre.sample - ( loop.end.sample + 1 );
						Position = new SoundTime( PositionPre.sampleRate, ( int )( loop.start.sample + PositionPre.sample - ( loop.end.sample + 1 ) ) );
					}
				}
				else
				{
					if( Position.sample >= waveform.format.samples )
					{
						Logger.Debug( "Loop " + Position.sample + " to " + ( PositionPre.sample - waveform.format.samples ) );
						
						//Position.sample = PositionPre.sample - waveform.format.samples;
						Position = new SoundTime( Position.sampleRate, ( int )( PositionPre.sample - waveform.format.samples ) );
					}
				}
			}
			
			if( Position.sample + 1 < waveform.format.samples )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					aSoundBuffer[i] = MeanInterpolation.Calculate( waveform, i, Position.sample );
				}
			}
			else if( Position.sample < waveform.format.samples )
			{
				for( int i = 0; i < aChannels; i++ )
				{
					if( loop.length.sample > 0 )
					{
						aSoundBuffer[i] = MeanInterpolation.Calculate( waveform, i, Position.sample, loop.start.sample );
					}
					else
					{
						aSoundBuffer[i] = waveform.reader.GetSample( i, ( int )Position.sample );
					}
				}
			}
			else // End position.
			{
				return true;
			}
			
			PositionPre = Position;
			//Position.sample += ( double )Position.sampleRate / ( double )aSampleRate;
			//Elapsed.sample += ( double )Elapsed.sampleRate / ( double )aSampleRate;
			Position = Position + GetOneSample( aSampleRate );
			Elapsed = Elapsed + GetOneSample( aSampleRate );

			return false;
		}

		public SoundTime GetOneSample( int aSampleRate )
		{
			if( oneSampelList.ContainsKey( aSampleRate ) == false )
			{
				oneSampelList.Add( aSampleRate, new SoundTime( aSampleRate, 1 ) );
			}
			
			return oneSampelList[aSampleRate];
		}
	}
}
