using System;

namespace Monoamp.Common.Struct
{
	public class LoopInformation
	{
		public readonly SoundTime start;
		public readonly SoundTime end;
		public readonly SoundTime length;

		public readonly string[] stringArrayStart;
		public readonly string[] stringArrayEnd;
		public readonly string[] stringArrayLength;

		public LoopInformation( int aSampleRate, int aSampleStart, int aSampleEnd )
		{
			start = new SoundTime( aSampleRate, aSampleStart );
			end = new SoundTime( aSampleRate, aSampleEnd );
			length = new SoundTime( aSampleRate, aSampleEnd - aSampleStart + 1 );

			if( start.sample < 0 || end.sample < 0 )
			{
				length.sample = -1;
			}

			stringArrayStart = new string[3];
			stringArrayEnd = new string[3];
			stringArrayLength = new string[3];

			stringArrayStart[0] = start.sample.ToString();
			stringArrayEnd[0] = end.sample.ToString();
			stringArrayLength[0] = length.sample.ToString();

			stringArrayStart[1] = start.Seconds.ToString();
			stringArrayEnd[1] = end.Seconds.ToString();
			stringArrayLength[1] = length.Seconds.ToString();

			stringArrayStart [2] = start.MMSSmmm;
			stringArrayEnd [2] = end.MMSSmmm;
			stringArrayLength [2] = length.MMSSmmm;
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
