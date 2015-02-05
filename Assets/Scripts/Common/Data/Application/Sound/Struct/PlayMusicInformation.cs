using System;

using Monoamp.Common.Data.Application.Music;

namespace Monoamp.Common.Struct
{
	public class PlayMusicInformation
	{
		public bool isSelected;
		public IMusic music;
		public LoopInformation loopPoint;
		
		public PlayMusicInformation( bool aIsSelected, IMusic aMusic, LoopInformation aLoopPoint )
		{
			isSelected = aIsSelected;
			music = aMusic;
			loopPoint = aLoopPoint;
		}
	}
}
