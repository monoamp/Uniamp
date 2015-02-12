using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Application.Sound
{
	public abstract class AWaveformReader
	{
		public struct Int24
		{
			public const int MinValue = -8388608;
			public const int MaxValue = 8388607;
		}
		
		protected const int LENGTH_BUFFER = 1024 * 4;

		public readonly WaweformFormat format;
		public readonly string filePath;
		public readonly int basePosition;
		
		private readonly int bufferLength;
		private readonly float[][] sampleArray;

		private int startPosition;

		private object objectLock;

		protected AWaveformReader( WaweformFormat aFormat, string aFilePath, int aBasePosition, bool aIsOnMemory )
		{
			objectLock = new object();

			format = aFormat;
			filePath = aFilePath;
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
			
			for( int i = 0; i < format.channels; i++ )
			{
				sampleArray[i] = new float[bufferLength];
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

		private void ReadSampleArray( int aPointSample )
		{
			if( filePath != null )
			{
				using ( FileStream u = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
				{
					AByteArray lByteArray = ConstructByteArray( u );
					
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
		
		protected abstract AByteArray ConstructByteArray( FileStream aFileStream );
		
		private void ReadSampleArray16( AByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( basePosition + 2 * format.channels * aPositionSample );
			
			for( int i = 0; i < bufferLength && i < format.samples - aPositionSample; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					Int32 lSample = aByteArray.ReadInt16();
					sampleArray[j][i] = ( float )lSample / ( float )Int16.MaxValue;
				}
			}
		}
		
		private void ReadSampleArray24( AByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( basePosition + 3 * format.channels * aPositionSample );
			
			for( int i = 0; i < bufferLength && i < format.samples - aPositionSample; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					Int32 lSample = aByteArray.ReadInt24();
					sampleArray[j][i] = ( float )lSample / ( float )Int24.MaxValue;
				}
			}
		}
	}
	
	public class WaveformReaderAiff : AWaveformReader
	{
		public WaveformReaderAiff( WaweformFormat aFormat, string aName, int aPosition, bool aIsOnMemory )
			: base( aFormat, aName, aPosition, aIsOnMemory )
		{
			
		}
		
		protected override AByteArray ConstructByteArray( FileStream aFileStream )
		{
			return new ByteArrayBig( aFileStream );
		}
	}
	
	public class WaveformReaderWave : AWaveformReader
	{
		public WaveformReaderWave( WaweformFormat aFormat, string aName, int aPosition, bool aIsOnMemory )
			: base( aFormat, aName, aPosition, aIsOnMemory )
		{
			
		}
		
		protected override AByteArray ConstructByteArray( FileStream aFileStream )
		{
			return new ByteArrayLittle( aFileStream );
		}
	}
}
