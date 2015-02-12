using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.Data.Standard.Form;
using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Struct;
using Monoamp.Common.system.io;
using Monoamp.Common.Utility;

namespace Monoamp.Common.Data.Application.Music
{
	public class MusicAiff : MusicPcm
	{
		public MusicAiff( string aPathFile )
			: base( PoolCollection.GetFormAiff( aPathFile ) )
		{

		}
	}
}
