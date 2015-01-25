using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public struct Int24
	{
		public const int MinValue = -8388608;
		public const int MaxValue = 8388607;
	}

	public abstract class MusicPcm : IMusic
	{
		protected const int LENGTH_BUFFER = 1024 * 4;

		public int Channels{ get; protected set; }
		public SoundTime Length{ get; protected set; }

		protected List<List<LoopInformation>> Loop{ get; set; }
		protected string nameFile{ get; set; }
		protected int bytePosition{ get; set; }
		protected int byteSize{ get; set; }
		protected int SampleBits{ get; set; }
		protected float[][] SampleArray{ get; set; }
		protected int StartPosition{ get; set; }
		protected int LengthBuffer{ get; set; }

		public int Samples{ get{ return ( int )Length.sample; } }
		public int SampleRate{ get{ return ( int )Length.sampleRate; } }

		public int GetCountLoopX()
		{
			return Loop.Count;
		}
		
		public int GetCountLoopY( int aIndexX )
		{
			if( aIndexX < GetCountLoopX() )
			{
				return Loop[aIndexX].Count;
			}
			else
			{
				return 0;
			}
		}
		
		public LoopInformation GetLoop( int aIndexX, int aIndexY )
		{
			if( aIndexX < GetCountLoopX() && aIndexY < GetCountLoopY( aIndexX ) )
			{
				return Loop[aIndexX][aIndexX];
			}
			else
			{
				return new LoopInformation( 44100, -1, -1 );
			}
		}

		public float GetSample( int aChannel, int aPositionSample )
		{
			if( aPositionSample < StartPosition || aPositionSample >= StartPosition + LengthBuffer )
			{
				StartPosition = aPositionSample;
				
				ReadSampleArray( StartPosition );
			}

			if( aPositionSample - StartPosition < 0 && aPositionSample - StartPosition >= SampleArray[aChannel].Length )
			{
				UnityEngine.Debug.LogError( "Start:" + StartPosition + ", Position:" + aPositionSample );
			}

			return SampleArray[aChannel % Channels][aPositionSample - StartPosition];
		}
		
		private void ReadSampleArray( int aPointSample )
		{
			if( nameFile != null )
			{
				using ( FileStream u = new FileStream( nameFile, FileMode.Open, FileAccess.Read ) )
				{
					ByteArray lByteArray = ConstructByteArray( u );
					
					switch( SampleBits )
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
			aByteArray.SetPosition( bytePosition + 2 * Channels * aPositionSample );
			
			for( int i = 0; i < LengthBuffer && i < ( int )Length.sample - aPositionSample; i++ )
			{
				for( int j = 0; j < Channels; j++ )
				{
					Int32 sample = aByteArray.ReadInt16();
					SampleArray[j][i] = ( float )sample / ( float )Int16.MaxValue;
				}
			}
		}
		
		private void ReadSampleArray24( ByteArray aByteArray, int aPositionSample )
		{
			aByteArray.SetPosition( bytePosition + 3 * Channels * aPositionSample );
			
			for( int i = 0; i < LengthBuffer && i < ( int )Length.sample - aPositionSample; i++ )
			{
				for( int j = 0; j < Channels; j++ )
				{
					Int32 sample = aByteArray.ReadInt24();
					SampleArray[j][i] = ( float )sample / ( float )Int24.MaxValue;
				}
			}
		}
	}
}
