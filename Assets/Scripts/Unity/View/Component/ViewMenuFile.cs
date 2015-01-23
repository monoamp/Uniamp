using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class ViewMenuFile : IView
	{
		public Rect rectMenu;
		private bool isShowMenu;
		private DialogDirectorySelect windowDirectorySelector;
		
		private DirectoryInfo directoryInfo;
		private DirectoryInfo directoryInfoRoot;
		
		public Rect Rect{ get; set; }

		public ViewMenuFile()
		{
			rectMenu = new Rect( 0, GuiStyleSet.StyleMenu.bar.fixedHeight, 100.0f, 200.0f );
			isShowMenu = false;
		}

		public void Awake()
		{
			isShowMenu = true;
			directoryInfo = new DirectoryInfo( Application.streamingAssetsPath );
			directoryInfoRoot = new DirectoryInfo( Application.streamingAssetsPath );
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
					ViewDirectoryTree lViewDirectoryTree = new ViewDirectoryTree( directoryInfoRoot, directoryInfo );
					
					windowDirectorySelector = new DialogDirectorySelect( ChangeDirectoryInput, lViewDirectoryTree, directoryInfo );

					isShowMenu = false;
				}
				if( GUILayout.Button( new GUIContent( "Output", "StyleMenu.Item" ), GuiStyleSet.StyleMenu.item ) == true )
				{
					ViewDirectoryTree lViewDirectoryTree = new ViewDirectoryTree( directoryInfoRoot, directoryInfo );
					
					windowDirectorySelector = new DialogDirectorySelect( ChangeDirectoryOutput, lViewDirectoryTree, directoryInfo );
					
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
			directoryInfo = aDirectoryInfo;
			//setDirectoryInfo( directoryInfo );
            Debug.Log( "Change Input" );
			windowDirectorySelector = null;
		}
		
		private void ChangeDirectoryOutput( DirectoryInfo aDirectoryInfo )
		{
			directoryInfo = aDirectoryInfo;
			//setDirectoryInfo( directoryInfo );
			Debug.Log( "Change Output" );
			windowDirectorySelector = null;
		}
	}
}
