using UnityEngine;

using System;

using Unity.Data;
using Unity.GuiStyle;

namespace Unity.View
{
	public abstract class AMenuItem
	{
		public readonly string title;
		
		public AMenuItem( string aTitle )
		{
			title = aTitle;
		}
		
		public float GetWidth()
		{
			return GuiStyleSet.StyleMenu.item.CalcSize( new GUIContent( title ) ).x;
		}

		public abstract void Select();
		public abstract void OnGUI();
	}
}
