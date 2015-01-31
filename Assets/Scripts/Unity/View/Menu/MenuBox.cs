using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;

namespace Unity.View
{
	public class MenuBox
	{
		public Rect rect;
		
		public readonly string title;

		protected readonly List<IMenuItem> menuItemList;

		private bool isShow;

		public MenuBox( string aTitle )
		{
			title = aTitle;
			menuItemList = new List<IMenuItem>();
		}

		public float GetWidth()
		{
			float lWidthMax = 0.0f;

			foreach( IMenuItem l in menuItemList )
			{
				if( l.GetWidth() > lWidthMax )
				{
					lWidthMax = l.GetWidth();
				}
			}

			return lWidthMax;
		}

		public void Select()
		{
			isShow = true;
		}

		public void OnGUI()
		{
			if( isShow == true )
			{
				GUI.Window( 0, rect, SelectItemWindow, "", GuiStyleSet.StyleMenu.window );
			}
			
			if( Input.GetMouseButtonDown( 0 ) == true )
			{
				float lY = Screen.height - 1 - Input.mousePosition.y;
				
				if( Input.mousePosition.x < rect.x || Input.mousePosition.x >= rect.x + rect.width ||
				   lY < rect.y || lY >= rect.y + rect.height )
				{
					isShow = false;
				}
			}
			
			foreach( IMenuItem l in menuItemList )
			{
				l.OnGUI();
			}
		}

		private void SelectItemWindow( int windowID )
		{
			GUILayout.BeginVertical();
			{
				foreach( IMenuItem l in menuItemList )
				{
					if( GUILayout.Button( new GUIContent( l.Title, "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
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
