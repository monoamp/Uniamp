using System;

using Monoamp.Common.Data.Application.Music;

namespace Monoamp.Common.Struct
{
	public class InputMusicInformation
	{
		public bool isSelected;
		public IMusic music;
		public double progress;
		
		public InputMusicInformation( bool aIsSelected, IMusic aMusic, double aProgress )
		{
			isSelected = aIsSelected;
			music = aMusic;
			progress = aProgress;
		}
	}
}
