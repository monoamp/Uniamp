using System;

namespace Monoamp.Common.Struct
{
	public struct LoopInformation
	{
		public readonly SoundTime start;
		public readonly SoundTime end;
		public readonly SoundTime length;

		public LoopInformation( int aSampleRate, int aSampleStart, int aSampleEnd )
		{
			start = new SoundTime( aSampleRate, aSampleStart );
			end = new SoundTime( aSampleRate, aSampleEnd );
			length = new SoundTime( aSampleRate, aSampleEnd - aSampleStart + 1 );

			if( start.sample < 0 || end.sample < 0 )
			{
				length.sample = -1;
			}
		}
	}

	public class SoundTime
	{
		public readonly int sampleRate;
		public double sample;

		public SoundTime( int aSampleRate, int aSample )
		{
			sampleRate = aSampleRate;
			sample = ( double )aSample;
		}

		public int MilliSecond{ get { return ( int )( sample / sampleRate % 1.0d * 1000 ); } }
		public int Second{ get { return ( int )sample / sampleRate % 60; } }
		public double Seconds{ get { return sample / sampleRate; } }
		public int Minute{ get { return ( int )sample / sampleRate / 60; } }
		public int Hour{ get { return ( int )sample / sampleRate / 3600; } }
		public string MMSSmmm{ get { return Minute.ToString( "D2" ) + ":" + Second.ToString( "D2" ) + "." + MilliSecond.ToString( "D3"); } }
		public string MMSS{ get { return Minute.ToString() + ":" + Second.ToString( "D2" ); } }

		public string String
		{
			get
			{
				if( sample > 0 )
				{
					return sample.ToString();
				}
				else
				{
					return "-";
				}
			}
		}
	}
}
