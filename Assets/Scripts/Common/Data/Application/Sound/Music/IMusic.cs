using System;
using System.Collections.Generic;

using Monoamp.Common.Struct;

namespace Monoamp.Common.Data.Application.Music
{
	public interface IMusic
	{
		int Channels{ get; }
		SoundTime Sample{ get; }
		int Samples{ get; }
		int SampleRate{ get; }
		List<List<LoopInformation>> Loop{ get; }
	}
}
