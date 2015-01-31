using System;

namespace Unity.View
{
	public interface IMenuItem
	{
		string Title{ get; }
		
		float GetWidth();
		void Select();
		void OnGUI();
	}
}
