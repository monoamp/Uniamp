using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;

namespace Unity.View
{
	public class MenuBox
	{
		public readonly string title;
		public Rect rectMenu;

		private bool isShow;
		private IMenuItem[] menuItemArray;

		public MenuBox( string aTitle, IMenuItem[] aMenuItemArray )
		{
			title = aTitle;
			menuItemArray = aMenuItemArray;
		}

		public void Select()
		{
			isShow = true;
		}

		public void OnGUI()
		{
			if( isShow == true )
			{
				GUI.Window( 0, rectMenu, SelectItemWindow, "", GuiStyleSet.StyleMenu.window );
			}
			
			if( Input.GetMouseButtonDown( 0 ) == true )
			{
				float lY = Screen.height - 1 - Input.mousePosition.y;
				
				if( Input.mousePosition.x < rectMenu.x || Input.mousePosition.x >= rectMenu.x + rectMenu.width ||
				   lY < rectMenu.y || lY >= rectMenu.y + rectMenu.height )
				{
					isShow = false;
				}
			}
			
			foreach( IMenuItem l in menuItemArray )
			{
				l.OnGUI();
			}
		}

		private void SelectItemWindow( int windowID )
		{
			GUILayout.BeginVertical();
			{
				foreach( IMenuItem l in menuItemArray )
				{
					if( GUILayout.Button( new GUIContent( l.title, "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
					{
						l.Select();
						
						isShow = false;
					}
				}
			}
			GUILayout.EndVertical();
		}
	}
}
