using System;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Data.Application.Waveform;

namespace Monoamp.Common.Component.Sound.Utility
{
	public static class MeanInterpolation
	{
		/*
		public static float Calculate( MusicPcm aMusic, int aChannel, double aSampleCurrent )
		{
			float a = aMusic.GetSample( aChannel, ( int )aSampleCurrent );
			float b = aMusic.GetSample( aChannel, ( int )aSampleCurrent + 1 );
			double positionDifference = aSampleCurrent - ( int )aSampleCurrent;

			return ( float )( a + ( b - a ) * positionDifference );
		}

		public static float Calculate( MusicPcm aMusic, int aChannel, double aSampleCurrent, double aSampleLoopStart )
		{
			float a = aMusic.GetSample( aChannel, ( int )aSampleCurrent );
			float b = aMusic.GetSample( aChannel, ( int )aSampleLoopStart );
			double positionDifference = aSampleCurrent - ( int )aSampleCurrent;

			return ( float )( a + ( b - a ) * positionDifference );
		}
*/
		public static float Calculate( WaveformPcm aWaveformBase, int aChannel, double aSampleCurrent )
		{
			float a = aWaveformBase.data.GetSample( aChannel, ( int )aSampleCurrent );
			float b = aWaveformBase.data.GetSample( aChannel, ( int )aSampleCurrent + 1 );
			double positionDifference = aSampleCurrent - ( int )aSampleCurrent;

			return ( float )( a + ( b - a ) * positionDifference );
		}

		public static float Calculate( WaveformPcm aWaveformBase, int aChannel, double aSampleCurrent, double aSampleLoopStart )
		{
			float a = aWaveformBase.data.GetSample( aChannel, ( int )aSampleCurrent );
			float b = aWaveformBase.data.GetSample( aChannel, ( int )aSampleLoopStart );
			double positionDifference = aSampleCurrent - ( int )aSampleCurrent;

			return ( float )( a + ( b - a ) * positionDifference );
		}
	}
}
