using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Waveform
{
	public class WaveformWave : IWaveform
	{
		public WaveformWave( string aPathFile )
			: this( PoolCollection.GetRiffWave( aPathFile ) )
		{
			
		}

		public WaveformWave( RiffWaveRiff aRiffFile )
			: base()
		{
			RiffWaveData lRiffWaveData = ( RiffWaveData )aRiffFile.GetChunk( RiffWaveData.ID );
			int lPosition = ( int )lRiffWaveData.position;
			int lLength = ( int )lRiffWaveData.Size;
			
			RiffWaveFmt_ lRiffWaveFmt_ = ( RiffWaveFmt_ )aRiffFile.GetChunk( RiffWaveFmt_.ID );
			int lChannels = lRiffWaveFmt_.channels;
			int lSampleRate = ( int )lRiffWaveFmt_.samplesPerSec;
			int lSampleBits = lRiffWaveFmt_.bitsPerSample;
			int lSamples = lLength / ( lSampleBits / 8 ) / lChannels;

			format = new FormatWaweform( lChannels, lSamples, lSampleRate, lSampleBits );
			data = new WaveformData( format, null, aRiffFile.name, lPosition );
		}
	}
}
