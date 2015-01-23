using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Application.Waveform
{
	public class FormatWaweform
	{
		public readonly int channels;
		public readonly int samples;
		public readonly int sampleRate;
		public readonly int sampleBits;

		public FormatWaweform( int aChannels, int aSamples, int aSampleRate, int aSampleBits )
		{
			channels = aChannels;
			samples = aSamples;
			sampleRate = aSampleRate;
			sampleBits = aSampleBits;
		}
	}

	public class WaveformData
	{
		public readonly FormatWaweform format;
		public readonly string name;
		public readonly int position;

		private float[][] sampleArray;
		private Int32[][] sampleDataArray;
		private int startPosition;

		private int bufferLength;

		public WaveformData( FormatWaweform aFormat, ByteArray aByteArray, string aName, int aPosition )
		{
			format = aFormat;
			name = aName;
			position = aPosition;

			bufferLength = format.samples;

			sampleArray = new float[format.channels][];
			sampleDataArray = new Int32[format.channels][];

			startPosition = 0x7FFFFFFF;
		}

		public void Cache()
		{
			GetSample( 0, 0 );
		}

		public float GetSample( int aChannel, int aPositionSample )
		{
			if( aPositionSample < startPosition || aPositionSample >= startPosition + bufferLength )
			{
				UnityEngine.Debug.Log( name );

				for( int i = 0; i < format.channels; i++ )
				{
					sampleArray[i] = new float[bufferLength];
					sampleDataArray[i] = new Int32[bufferLength];
				}

				startPosition = aPositionSample;

				ReadSampleArray( startPosition );
			}

			if( aChannel < sampleArray.Length )
			{
				return sampleArray[aChannel][aPositionSample - startPosition];
			}
			else
			{
				return sampleArray[0][aPositionSample - startPosition];
			}
		}

		public Int32 GetSampleData( int aChannel, int aPositionSample )
		{
			if( aPositionSample < startPosition || aPositionSample >= startPosition + bufferLength )
			{
				for( int i = 0; i < format.channels; i++ )
				{
					sampleArray[i] = new float[bufferLength];
					sampleDataArray[i] = new Int32[bufferLength];
				}

				startPosition = aPositionSample;

				ReadSampleArray( startPosition );
			}

			return sampleDataArray[aChannel][aPositionSample - startPosition];
		}

		private void ReadSampleArray( int aPointSample )
		{
			if( name != null )
			{
				using ( FileStream u = new FileStream( name, FileMode.Open, FileAccess.Read ) )
				{
					ByteArray l = new ByteArrayLittle( u );

					switch( format.sampleBits )
					{
					case 16:
						ReadSampleArray16( l, aPointSample );
						break;

					case 24:
						ReadSampleArray24( l, aPointSample );
						break;

					default:
						break;
					}
				}
			}
		}

		private void ReadSampleArray16( ByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( position + 2 * format.channels * aPositionSample );

			for( int i = 0; i < bufferLength && i < format.samples - aPositionSample; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					sampleDataArray[j][i] = aByteArray.ReadInt16();
					sampleArray[j][i] = ( float )sampleDataArray[j][i] / ( float )0x8000;
				}
			}
		}

		private void ReadSampleArray24( ByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( position + 3 * format.channels * aPositionSample );

			for( int i = 0; i < bufferLength && i < format.samples - aPositionSample; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					sampleDataArray[j][i] = aByteArray.ReadInt24();
					sampleArray[j][i] = ( float )sampleDataArray[j][i] / ( float )0x800000;
				}
			}
		}
	}

	public class IWaveform
	{
		public FormatWaweform format{ get; protected set; }
		public WaveformData data{ get; protected set; }
	}
}
