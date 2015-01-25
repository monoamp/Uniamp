using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff.Wave;

namespace Monoamp.Common.Utility
{
	public static class PoolCollection
	{
		private static readonly Pool poolFormAiff;
		private static readonly Pool poolRiffWave;

		static PoolCollection()
		{
			poolFormAiff = new Pool( ( FileStream aFileStream ) => { return new FormAiffForm( aFileStream ); } );
			poolRiffWave = new Pool( ( FileStream aFileStream ) => { return new RiffWaveRiff( aFileStream ); } );
		}

		public static FormAiffForm GetFormAiff( string aPathFile )
		{
			return ( FormAiffForm )poolFormAiff.Get( aPathFile );
		}
		
		public static RiffWaveRiff GetRiffWave( string aPathFile )
		{
			return ( RiffWaveRiff )poolRiffWave.Get( aPathFile );
		}
	}
}
