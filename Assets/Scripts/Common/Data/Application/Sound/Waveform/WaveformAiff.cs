using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Waveform
{
	public class WaveformAiff : WaveformPcm
	{
		public WaveformAiff( string aPathFile )
			: base( PoolCollection.GetFormAiff( aPathFile ) )
		{
			
		}
	}
}
