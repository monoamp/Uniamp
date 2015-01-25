using System;
using System.Collections.Generic;

using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public interface IMusic
	{
		SoundTime Length{ get; }

		int GetCountLoopX();
		int GetCountLoopY( int aIndexX );
		LoopInformation GetLoop( int aIndexX, int aIndexY );
	}
}
