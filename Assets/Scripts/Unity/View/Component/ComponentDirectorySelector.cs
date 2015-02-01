using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ComponentDirectorySelector : IView
	{
		private ComponentDirectoryTree viewDirectoryTree;

		private Vector2 positionScrollDirectory;

		private DirectoryInfo directoryInfoSelected;

		public delegate void CloseWindow( DirectoryInfo aDirectoryInfo );
		private CloseWindow closeWindow;
		
		private List<DirectoryInfo> directoryInfoRecentList;

		public Rect Rect{ get; set; }

		public ComponentDirectorySelector( CloseWindow aCloseWindow, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			closeWindow = aCloseWindow;
			viewDirectoryTree = new ComponentDirectoryTree( aDirectoryInfoRecentList[0].Root, aDirectoryInfoRecentList[0] );
			directoryInfoRecentList = aDirectoryInfoRecentList;
			directoryInfoSelected = aDirectoryInfoRecentList[0];

			int position = viewDirectoryTree.GetItemPositionDisplay() - 2;
			
			float lHeightLine = GuiStyleSet.StyleList.toggleLine.CalcSize( new GUIContent( "" ) ).y;

			positionScrollDirectory.y = ( float )position * lHeightLine;
		}

		public void Select()
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
				GUILayout.Label( new GUIContent( "", "StyleGeneral.PartitionHorizontal" ), GuiStyleSet.StyleGeneral.partitionHorizontal );
				DisplayRecentDirectories();
				GUILayout.FlexibleSpace();
				DisplayButton();
			}
			GUILayout.EndVertical();
		}

		private void DisplayDirectoryTree()
		{
			DirectoryInfo lDirectoryInfo = viewDirectoryTree.DirectoryInfoSelected;

			positionScrollDirectory = GUILayout.BeginScrollView( positionScrollDirectory, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar );
			{
				viewDirectoryTree.OnGUI();
			}
			GUILayout.EndScrollView();

			if( viewDirectoryTree.DirectoryInfoSelected != lDirectoryInfo )
			{
				directoryInfoSelected = viewDirectoryTree.DirectoryInfoSelected;
			}
		}
		
		private void DisplayRecentDirectories()
		{
			GUILayout.Label( "Open Recent", GuiStyleSet.StyleGeneral.label );

			GUILayout.BeginVertical();
			{
				foreach( DirectoryInfo l in directoryInfoRecentList )
				{
					if( l == directoryInfoSelected )
					{
						GUILayout.Toggle( true, new GUIContent( l.Name, l.FullName ), GuiStyleSet.StyleList.toggleLine );
					}
					else
					{
						bool lIsSeledted = GUILayout.Toggle( false, new GUIContent( l.Name, l.FullName ), GuiStyleSet.StyleList.toggleLine );
							
						if( lIsSeledted == true )
						{
							directoryInfoSelected = l;
							viewDirectoryTree.DirectoryInfoSelected = directoryInfoSelected;
						}
					}
				}
			}
			GUILayout.EndVertical();
		}

		private void DisplayButton()
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				
				if( GUILayout.Button( new GUIContent( "OK", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
				{
					closeWindow( directoryInfoSelected );
				}

				if( GUILayout.Button( new GUIContent( "Cancel", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
				{
					closeWindow( null );
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}