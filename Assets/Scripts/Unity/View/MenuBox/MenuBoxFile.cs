using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class MenuBoxFile : IView
	{
		public Rect rectMenu;
		private bool isShow;
		private MenuItemChangeDirectory menuItemChangeDirectory;

		public Rect Rect{ get; set; }

		public MenuBoxFile( MenuItemChangeDirectory aMenuItemChangeDirectory )
		{
			menuItemChangeDirectory = aMenuItemChangeDirectory;
			rectMenu = new Rect( 0, GuiStyleSet.StyleMenu.bar.fixedHeight, 100.0f, 200.0f );
			isShow = false;
		}

		public void Awake()
		{
			isShow = true;
		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			
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

			menuItemChangeDirectory.OnGUI();
		}

		public void SelectItemWindow( int windowID )
		{
			GUILayout.BeginVertical();
			{
				if( GUILayout.Button( new GUIContent( "Input", "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
				{
					menuItemChangeDirectory.Select();

					isShow = false;
				}
			}
			GUILayout.EndVertical();
		}

		public void OnRenderObject()
		{
			
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

		}

		public void OnApplicationQuit()
		{

		}
	}
}
