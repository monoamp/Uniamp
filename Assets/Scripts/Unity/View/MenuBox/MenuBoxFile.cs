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
		public string title{ get; private set; }
		private bool isShow;
		public Rect rectMenu;
		private MenuItemChangeDirectory menuItemChangeDirectoryInput;
		private MenuItemChangeDirectory menuItemChangeDirectoryOutput;

		public Rect Rect{ get; set; }

		public MenuBoxFile( MenuItemChangeDirectory aMenuItemChangeDirectoryInput, MenuItemChangeDirectory aMenuItemChangeDirectoryOutput )
		{
			title = "File";
			menuItemChangeDirectoryInput = aMenuItemChangeDirectoryInput;
			menuItemChangeDirectoryOutput = aMenuItemChangeDirectoryOutput;
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
			
			menuItemChangeDirectoryInput.OnGUI();
			menuItemChangeDirectoryOutput.OnGUI();
		}

		public void SelectItemWindow( int windowID )
		{
			GUILayout.BeginVertical();
			{
				if( GUILayout.Button( new GUIContent( menuItemChangeDirectoryInput.title, "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
				{
					menuItemChangeDirectoryInput.Select();

					isShow = false;
				}
				
				if( GUILayout.Button( new GUIContent( menuItemChangeDirectoryOutput.title, "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
				{
					menuItemChangeDirectoryOutput.Select();
					
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
