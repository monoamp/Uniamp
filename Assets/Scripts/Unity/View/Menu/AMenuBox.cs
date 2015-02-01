using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public abstract class AMenuBox
	{
		public Rect rect;
		
		public readonly string title;

		protected readonly List<AMenuItem> menuItemList;

		private bool isShow;

		public AMenuBox( string aTitle )
		{
			title = aTitle;
			menuItemList = new List<AMenuItem>();
		}
		
		protected Dictionary<string, string> ReadDictionaryLanguage( string aFilePathLanguage )
		{
			Dictionary<string, string> lDictionaryDescription = new Dictionary<string, string>();
			
			try
			{
				using( StreamReader u = new StreamReader( aFilePathLanguage ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( line.IndexOf( "//" ) != 0 && line.Split( ':' ).Length == 2 )
						{
							string key = line.Split( ':' )[0];
							string description = line.Split( ':' )[1];
							
							lDictionaryDescription.Add( key, description );
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakError( "Exception:" + aExpection );
			}
			
			return lDictionaryDescription;
		}

		public float GetWidth()
		{
			float lWidthMax = 0.0f;

			foreach( AMenuItem l in menuItemList )
			{
				if( l.GetWidth() > lWidthMax )
				{
					lWidthMax = l.GetWidth();
				}
			}

			return lWidthMax;
		}
		
		public int GetCount()
		{
			return menuItemList.Count;
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
			
			foreach( AMenuItem l in menuItemList )
			{
				l.OnGUI();
			}
		}

		private void SelectItemWindow( int windowID )
		{
			GUILayout.BeginVertical();
			{
				foreach( AMenuItem l in menuItemList )
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
