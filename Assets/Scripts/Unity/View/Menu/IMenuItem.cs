using System;

namespace Unity.View
{
	public interface IMenuItem
	{
		string title{ get; }

		void Select();
		void OnGUI();
	}
}
