﻿using System;
using System.Collections.Generic;

using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public interface IMusic
	{
		string Name{ get; }
		SoundTime Length{ get; }
		LoopInformation Loop{ get; set; }

		int GetCountLoopX();
		int GetCountLoopY( int aIndexX );
		LoopInformation GetLoop( int aIndexX, int aIndexY );
	}
}
