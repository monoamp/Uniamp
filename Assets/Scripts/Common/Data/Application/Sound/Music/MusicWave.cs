using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Struct;
using Monoamp.Common.system.io;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Music
{
	public class MusicWave : MusicPcm
	{
		public MusicWave( string aPathFile )
			: base( PoolCollection.GetRiffWave( aPathFile ) )
		{
			
		}
	}
}
