using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Sound
{
	public class WaveformReaderPcm
	{
		public readonly WaweformFormat format;
		public readonly AWaveformReader reader;
	
		public WaveformReaderPcm( FormAiffForm aFormFile, bool aIsOnMemory )
		{
			FormAiffSsnd lSsndChunk = ( FormAiffSsnd )aFormFile.GetChunk( FormAiffSsnd.ID );
			int lPosition = ( int )( lSsndChunk.position + lSsndChunk.offset + 8 );
			int lLength = lSsndChunk.dataSize;
			
			FormAiffComm lChunkComm = ( FormAiffComm )aFormFile.GetChunk( FormAiffComm.ID );
			int lChannels = lChunkComm.numberOfChannels;
			int lSampleRate = ( int )lChunkComm.sampleRate;
			int lSampleBits = lChunkComm.bitsPerSamples;
			int lSamples = lLength / ( lSampleBits / 8 ) / lChannels;
			
			format = new WaweformFormat( lChannels, lSamples, lSampleRate, lSampleBits );
			reader = new WaveformReaderAiff( format, aFormFile.name, lPosition, aIsOnMemory );
		}

		public WaveformReaderPcm( RiffWaveRiff aRiffFile, bool aIsOnMemory )
		{
			RiffWaveData lRiffWaveData = ( RiffWaveData )aRiffFile.GetChunk( RiffWaveData.ID );
			int lPosition = ( int )lRiffWaveData.position;
			int lLength = ( int )lRiffWaveData.Size;
			
			RiffWaveFmt_ lRiffWaveFmt_ = ( RiffWaveFmt_ )aRiffFile.GetChunk( RiffWaveFmt_.ID );
			int lChannels = lRiffWaveFmt_.channels;
			int lSampleRate = ( int )lRiffWaveFmt_.samplesPerSec;
			int lSampleBits = lRiffWaveFmt_.bitsPerSample;
			int lSamples = lLength / ( lSampleBits / 8 ) / lChannels;
			
			format = new WaweformFormat( lChannels, lSamples, lSampleRate, lSampleBits );
			reader = new WaveformReaderWave( format, aRiffFile.name, lPosition, aIsOnMemory );
		}
	}
}
