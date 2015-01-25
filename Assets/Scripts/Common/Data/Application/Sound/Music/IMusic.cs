using System;
using System.Collections.Generic;

using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public interface IMusic
	{
		int Channels{ get; }
		SoundTime Length{ get; }
		int Samples{ get; }
		int SampleRate{ get; }

		int GetCountLoopX();
		int GetCountLoopY( int aIndexX );
		LoopInformation GetLoop( int aIndexX, int aIndexY );
	}
}
