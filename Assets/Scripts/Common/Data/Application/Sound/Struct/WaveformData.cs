using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Application.Sound
{
	public struct WaveformData
	{
		private readonly WaweformFormat format;
		private readonly sbyte[][] sampleByteArray;

		public WaveformData( WaweformFormat aFormat, AByteArray aByteArray, int aBasePosition )
		{
			format = aFormat;
			sampleByteArray = new sbyte[format.channels][];
			
			for( int i = 0; i < format.channels; i++ )
			{
				sampleByteArray[i] = new sbyte[format.samples];
			}
			
			ReadSampleArray( aByteArray, aBasePosition );
		}

		public sbyte GetSampleByte( int aChannel, int aPositionSample )
		{
			return sampleByteArray[aChannel % format.channels][aPositionSample];
		}

		private void ReadSampleArray( AByteArray aByteArray, int aBasePosition )
		{
			switch( format.sampleBits )
			{
				case 16:
					ReadSampleArray16( aByteArray, aBasePosition );
					break;
					
				case 24:
					ReadSampleArray24( aByteArray, aBasePosition );
					break;
					
				default:
					break;
			}
		}

		private void ReadSampleArray16( AByteArray aByteArray, int aBasePosition )
		{
			aByteArray.SetPosition( aBasePosition );
			
			for( int i = 0; i < format.samples; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					Int32 lSample = aByteArray.ReadInt16();
					sampleByteArray[j][i] = ( sbyte )( lSample >> 8 );
				}
			}
		}
		
		private void ReadSampleArray24( AByteArray aByteArray, int aBasePosition )
		{
			aByteArray.SetPosition( aBasePosition );
			
			for( int i = 0; i < format.samples; i++ )
			{
				for( int j = 0; j < format.channels; j++ )
				{
					Int32 lSample = aByteArray.ReadInt24();
					sampleByteArray[j][i] = ( sbyte )( lSample >> 16 );
				}
			}
		}
	}
}
