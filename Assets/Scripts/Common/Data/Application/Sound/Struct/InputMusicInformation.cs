using System;

using Monoamp.Common.Data.Application.Music;

namespace Monoamp.Common.Struct
{
	public class InputMusicInformation
	{
		public long timeStampTicks;
		public bool isSelected;
		public IMusic music;
		public double progress;
		
		public InputMusicInformation( long aTimeStampTicks, bool aIsSelected, IMusic aMusic, double aProgress )
		{
			timeStampTicks = aTimeStampTicks;
			isSelected = aIsSelected;
			music = aMusic;
			progress = aProgress;
		}
	}
}
