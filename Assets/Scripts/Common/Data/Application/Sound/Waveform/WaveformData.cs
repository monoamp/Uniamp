using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Application.Waveform
{
	public struct FormatWaweform
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

	public abstract class WaveformData
	{
		public struct Int24
		{
			public const int MinValue = -8388608;
			public const int MaxValue = 8388607;
		}
		
		protected const int LENGTH_BUFFER = 1024 * 4;

		public readonly FormatWaweform format;
		public readonly string name;
		public readonly int basePosition;
		
		private readonly int bufferLength;
		private readonly float[][] sampleArray;
		private readonly Int32[][] sampleDataArray;

		private int startPosition;

		private object objectLock;

		protected WaveformData( FormatWaweform aFormat, string aName, int aBasePosition, bool aIsOnMemory )
		{
			objectLock = new object();

			format = aFormat;
			name = aName;
			basePosition = aBasePosition;

			if( aIsOnMemory == true || LENGTH_BUFFER == 0 )
			{
				bufferLength = format.samples;
			}
			else
			{
				bufferLength = LENGTH_BUFFER;
			}

			sampleArray = new float[format.channels][];
			sampleDataArray = new Int32[format.channels][];
			
			for( int i = 0; i < format.channels; i++ )
			{
				sampleArray[i] = new float[bufferLength];
				sampleDataArray[i] = new Int32[bufferLength];
			}

			startPosition = int.MaxValue;
		}

		public float GetSample( int aChannel, int aPositionSample )
		{
			lock( objectLock )
			{
				if( aPositionSample >= format.samples )
				{
					return 0.0f;
				}

				if( aPositionSample < startPosition || aPositionSample >= startPosition + bufferLength )
				{
					startPosition = aPositionSample;
					
					ReadSampleArray( startPosition );
				}
				
				if( aPositionSample - startPosition < 0 && aPositionSample - startPosition >= sampleArray[aChannel].Length )
				{
					UnityEngine.Debug.LogError( "Start:" + startPosition + ", Position:" + aPositionSample );
				}
				
				return sampleArray[aChannel % format.channels][aPositionSample - startPosition];
			}
		}
		
		public Int32 GetSampleData( int aChannel, int aPositionSample )
		{
			if( aPositionSample < startPosition || aPositionSample >= startPosition + bufferLength )
			{
				startPosition = aPositionSample;
				
				ReadSampleArray( startPosition );
			}
			
			if( aPositionSample - startPosition < 0 && aPositionSample - startPosition >= sampleArray[aChannel].Length )
			{
				UnityEngine.Debug.LogError( "Start:" + startPosition + ", Position:" + aPositionSample );
			}
			
			return sampleDataArray[aChannel % format.channels][aPositionSample - startPosition];
		}

		private void ReadSampleArray( int aPointSample )
		{
			if( name != null )
			{
				using ( FileStream u = new FileStream( name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
				{
					ByteArray lByteArray = ConstructByteArray( u );
					
					switch( format.sampleBits )
					{
						case 16:
							ReadSampleArray16( lByteArray, aPointSample );
							break;
							
						case 24:
							ReadSampleArray24( lByteArray, aPointSample );
							break;
							
						default:
							break;
					}
				}
			}
		}
		
		protected abstract ByteArray ConstructByteArray( FileStream aFileStream );
		
		private void ReadSampleArray16( ByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( basePosition + 2 * format.channels * aPositionSample );
			
			for( int i = 0; i < bufferLength && i < format.samples - aPositionSample; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					Int32 lSample = aByteArray.ReadInt16();
					sampleDataArray[j][i] = lSample;
					sampleArray[j][i] = ( float )lSample / ( float )Int16.MaxValue;
				}
			}
		}
		
		private void ReadSampleArray24( ByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( basePosition + 3 * format.channels * aPositionSample );
			
			for( int i = 0; i < bufferLength && i < format.samples - aPositionSample; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					Int32 lSample = aByteArray.ReadInt24();
					sampleDataArray[j][i] = lSample;
					sampleArray[j][i] = ( float )lSample / ( float )Int24.MaxValue;
				}
			}
		}
	}
	
	public class WaveformDataAiff : WaveformData
	{
		public WaveformDataAiff( FormatWaweform aFormat, string aName, int aPosition )
			: base( aFormat, aName, aPosition, false )
		{
			
		}
		
		protected override ByteArray ConstructByteArray( FileStream aFileStream )
		{
			return new ByteArrayBig( aFileStream );
		}
	}
	
	public class WaveformDataWave : WaveformData
	{
		public WaveformDataWave( FormatWaweform aFormat, string aName, int aPosition, bool aIsOnMemory )
			: base( aFormat, aName, aPosition, aIsOnMemory )
		{
			
		}
		
		protected override ByteArray ConstructByteArray( FileStream aFileStream )
		{
			return new ByteArrayLittle( aFileStream );
		}
	}
}
