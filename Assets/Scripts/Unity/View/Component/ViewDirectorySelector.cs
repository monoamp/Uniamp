using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ViewDirectorySelector : IView
	{
		private ViewDirectoryTree viewDirectoryTree;

		private Vector2 positionScrollDirectory;
		private Vector2 positionScrollRecent;

		private DirectoryInfo directoryInfo;

		public delegate void CloseWindow( DirectoryInfo aDirectoryInfo );
		
		private CloseWindow closeWindow;

		public Rect Rect{ get; set; }

		public ViewDirectorySelector( CloseWindow aCloseWindow, ViewDirectoryTree aViewDirectoryTree, DirectoryInfo aDirectoryInfo )
		{
			closeWindow = aCloseWindow;
			viewDirectoryTree = aViewDirectoryTree;
			directoryInfo = aDirectoryInfo;

			int position = viewDirectoryTree.GetItemPositionDisplay() - 2;
			
			float lHeightLine = GuiStyleSet.StyleList.toggleLine.CalcSize( new GUIContent( "" ) ).y;

			positionScrollDirectory.y = ( float )position * lHeightLine;
		}

		public void Awake()
		{
			
		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			
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

        public void OnGUI()
		{
			GUILayout.BeginVertical();
			{
				GUILayout.TextArea( viewDirectoryTree.DirectoryInfoSelected.FullName, GuiStyleSet.StyleFolder.barAddress );
				DisplayDirectoryTree();
				GUILayout.FlexibleSpace();
				DisplayButton();
			}
			GUILayout.EndVertical();
		}

		private void DisplayDirectoryTree()
		{
			positionScrollDirectory = GUILayout.BeginScrollView( positionScrollDirectory, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar );
			{
				viewDirectoryTree.OnGUI();
			}
			GUILayout.EndScrollView();
		}
        
		private void DisplayButton()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				
				if( GUILayout.Button( new GUIContent( "OK", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
				{
					directoryInfo = viewDirectoryTree.DirectoryInfoSelected;
					closeWindow( directoryInfo );
				}

				if( GUILayout.Button( new GUIContent( "Cancel", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
				{
					closeWindow( directoryInfo );
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}
