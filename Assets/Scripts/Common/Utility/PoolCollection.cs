using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff.Wave;

namespace Monoamp.Common.Utility
{
	public static class PoolCollection
	{
		private static readonly Pool poolAif;
		private static readonly Pool poolWav;

		static PoolCollection()
		{
			poolAif = new Pool( ( FileStream aFileStream ) => { return new FormAiffForm( aFileStream ); } );
			poolWav = new Pool( ( FileStream aFileStream ) => { return new RiffWaveRiff( aFileStream ); } );
		}

		public static FormAiffForm GetAif( string aPathFile )
		{
			return ( FormAiffForm )poolAif.Get( aPathFile );
		}
		
		public static RiffWaveRiff GetWav( string aPathFile )
		{
			return ( RiffWaveRiff )poolWav.Get( aPathFile );
		}
	}
}
