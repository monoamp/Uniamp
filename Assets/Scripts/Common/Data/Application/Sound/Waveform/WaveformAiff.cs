using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Utility;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Application.Sound
{
	public class WaveformAiff : WaveformReaderPcm
	{
		public WaveformAiff( string aPathFile )
			: base( PoolCollection.GetFormAiff( aPathFile ), false )
		{
			
		}
	}
}
