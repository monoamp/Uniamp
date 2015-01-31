using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class MenuItemFile : IView
	{
		public Rect rectMenu;
		private bool isShowMenu;
		private DialogDirectorySelect windowDirectorySelector;

		public Rect Rect{ get; set; }
		
		private DirectoryInfo directoryInfo;
		private DirectoryInfo directoryInfoRoot;
		public delegate void SetDirectoryInfo( DirectoryInfo aDirectoryInfo );
		private SetDirectoryInfo setDirectoryInfo;
		private List<DirectoryInfo> directoryInfoRecentList;

		public MenuItemFile( DirectoryInfo aDirectoryInfoRoot, DirectoryInfo aDirectoryInfo, SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			rectMenu = new Rect( 0, GuiStyleSet.StyleMenu.bar.fixedHeight, 100.0f, 200.0f );
			isShowMenu = false;
			
			directoryInfo = aDirectoryInfo;
			directoryInfoRoot = aDirectoryInfoRoot;
			setDirectoryInfo = aSetDirectoryInfo;
			directoryInfoRecentList = aDirectoryInfoRecentList;
		}

		public void Awake()
		{
			isShowMenu = true;
		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			
		}

		public void OnGUI()
		{
			if( windowDirectorySelector != null )
			{
				windowDirectorySelector.OnGUI();
			}

			if( isShowMenu == true )
			{
				GUI.Window( 0, rectMenu, SelectItemWindow, "", GuiStyleSet.StyleMenu.window );
			}
			
			if( Input.GetMouseButtonDown( 0 ) == true )
			{
				float lY = Screen.height - 1 - Input.mousePosition.y;
				
				if( Input.mousePosition.x < rectMenu.x || Input.mousePosition.x >= rectMenu.x + rectMenu.width ||
				   lY < rectMenu.y || lY >= rectMenu.y + rectMenu.height )
				{
					isShowMenu = false;
				}
			}
		}

		public void SelectItemWindow( int windowID )
		{
			GUILayout.BeginVertical();
			{
				if( GUILayout.Button( new GUIContent( "Input", "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
				{
					ComponentDirectoryTree lViewDirectoryTree = new ComponentDirectoryTree( directoryInfoRoot, directoryInfo );
					
					windowDirectorySelector = new DialogDirectorySelect( ChangeDirectoryInput, lViewDirectoryTree, directoryInfo, directoryInfoRecentList );

					isShowMenu = false;
				}
			}
			GUILayout.EndVertical();
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

		}

		public void OnApplicationQuit()
		{

		}

		public void OnRenderObject()
		{

		}
		
		private void ChangeDirectoryInput( DirectoryInfo aDirectoryInfo )
		{
			Debug.Log( "Change Input" );
			windowDirectorySelector = null;
			setDirectoryInfo( aDirectoryInfo );
		}
	}
}
