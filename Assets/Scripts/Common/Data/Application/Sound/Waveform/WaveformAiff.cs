using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Waveform
{
	public class WaveformAiff : IWaveform
	{
		public WaveformAiff( string aPathFile )
			: this( PoolCollection.GetAif( aPathFile ) )
		{
			
		}

		public WaveformAiff( FormAiffForm aFormFile )
			: base()
		{
			FormAiffSsnd lSsndChunk = ( FormAiffSsnd )aFormFile.GetChunk( FormAiffSsnd.ID );
			int lPosition = ( int )( lSsndChunk.position + lSsndChunk.offset + 8 );
			int lLength = lSsndChunk.dataSize;
			
			FormAiffComm lChunkComm = ( FormAiffComm )aFormFile.GetChunk( FormAiffComm.ID );
			int lChannels = lChunkComm.numberOfChannels;
			int lSampleRate = ( int )lChunkComm.sampleRate;
			int lSampleBits = lChunkComm.bitsPerSamples;
			int lSamples = lLength / ( lSampleBits / 8 ) / lChannels;

			format = new FormatWaweform( lChannels, lSamples, lSampleRate, lSampleBits );
			data = new WaveformData( format, null, aFormFile.name, lPosition );
		}
	}
}
