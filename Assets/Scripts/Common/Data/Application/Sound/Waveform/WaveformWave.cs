using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Waveform
{
	public class WaveformWave : WaveformPcm
	{
		public WaveformWave( string aPathFile )
			: base( PoolCollection.GetRiffWave( aPathFile ) )
		{
			
		}
	}
}
