using System;

namespace Monoamp.Common.Struct
{
	public struct LoopInformation
	{
		public readonly SoundTime start;
		public readonly SoundTime end;
		public readonly SoundTime length;

		public LoopInformation( double aSampleRate, double aSampleStart, double aSampleEnd )
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

	public struct SoundTime
	{
		public readonly double sampleRate;
		public double sample;

		public int MilliSecond{ get { return ( int )( sample / sampleRate % 1.0d * 1000 ); } }
		public int Second{ get { return ( int )( sample / sampleRate % 60 ); } }
		//public double Seconds{ get { return sample / sampleRate; } }
		public int Minute{ get { return ( int )( sample / sampleRate / 60 ); } }
		public int Hour{ get { return ( int )( sample / sampleRate / 3600 ); } }
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

		public SoundTime( double aSampleRate, double aSample )
		{
			sampleRate = aSampleRate;
			sample = aSample;
		}

		public static SoundTime operator+( SoundTime a, SoundTime b )
		{
			return new SoundTime( a.sampleRate, a.sample + a.sampleRate / b.sampleRate );
		}

		public static double operator/( SoundTime a, SoundTime b )
		{
			return ( a.sample / a.sampleRate ) / ( b.sample / b.sampleRate );
		}
	}
}
