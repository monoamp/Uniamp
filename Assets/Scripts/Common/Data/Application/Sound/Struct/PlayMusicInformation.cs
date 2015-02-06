using System;

using Monoamp.Common.Data.Application.Music;

namespace Monoamp.Common.Struct
{
	public class PlayMusicInformation
	{
		public long timeStampTicks;
		public bool isSelected;
		public IMusic music;
		public LoopInformation loopPoint;
		
		public PlayMusicInformation( long aTimeStampTicks, bool aIsSelected, IMusic aMusic, LoopInformation aLoopPoint )
		{
			timeStampTicks = aTimeStampTicks;
			isSelected = aIsSelected;
			music = aMusic;
			loopPoint = aLoopPoint;
		}
	}
}
