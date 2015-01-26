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
		private readonly WaveformPcm waveform;
		
		public bool isLoop;
		public LoopInformation loop;

		public SoundTime Position{ get; private set; }
		public SoundTime PositionPre{ get; private set; }
		public SoundTime Elapsed{ get; private set; }

		public double PositionRate
		{
			get { return Position.sample / waveform.format.samples; }
 			set { PositionPre.sample = Position.sample = waveform.format.samples * value; }
		}

		public SynthesizerPcm( WaveformPcm aWaveform, LoopInformation aLoop )
		{
			waveform = aWaveform;

			Position = new SoundTime( 44100, 0 );
			PositionPre = new SoundTime( 44100, 0 );
			Elapsed = new SoundTime( 44100, 0 );
			
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
						
						Position.sample = loop.start.sample + PositionPre.sample - ( loop.end.sample + 1 );
					}
				}
				else
				{
					if( Position.sample >= waveform.format.samples )
					{
						Logger.Debug( "Loop " + Position.sample + " to " + ( PositionPre.sample - waveform.format.samples ) );
						
						Position.sample = PositionPre.sample - waveform.format.samples;
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
						aSoundBuffer[i] = waveform.data.GetSample( i, ( int )Position.sample );
					}
				}
			}
			else // End position.
			{
				return true;
			}
			
			PositionPre.sample = Position.sample;
			Position.sample += ( double )waveform.format.sampleRate / ( double )aSampleRate;
			Elapsed.sample += ( double )waveform.format.sampleRate / ( double )aSampleRate;

			return false;
		}
	}
}
