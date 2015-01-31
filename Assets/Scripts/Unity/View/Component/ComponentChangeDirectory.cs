using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Component.Sound;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;

namespace Unity.View
{
	public class ComponentChangeDirectory : IView
	{
		private DialogDirectorySelect dialogDirectorySelector;
		
		private DirectoryInfo directoryInfo;
		public delegate void SetDirectoryInfo( DirectoryInfo aDirectoryInfo );
		private SetDirectoryInfo setDirectoryInfo;
		private List<DirectoryInfo> directoryInfoRecentList;

		public Rect Rect{ get; set; }

		public ComponentChangeDirectory( DirectoryInfo aDirectoryInfo, SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			directoryInfo = aDirectoryInfo;
			setDirectoryInfo = aSetDirectoryInfo;
			directoryInfoRecentList = aDirectoryInfoRecentList;
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
			if( dialogDirectorySelector != null )
			{
				dialogDirectorySelector.OnGUI();
			}
			
			float lWidth = GuiStyleSet.StyleFolder.buttonFolder.CalcSize( new GUIContent( "" ) ).x;

			GUILayout.BeginHorizontal( GuiStyleSet.StyleFolder.background );
			{
				GUILayout.TextArea( directoryInfo.FullName, GuiStyleSet.StyleFolder.text, GUILayout.Width( Screen.width / 2.0f - lWidth ) );
				
				if( GUILayout.Button( new GUIContent( "", "StyleFolder.ButtonFolder" ), GuiStyleSet.StyleFolder.buttonFolder ) == true )
				{
					ComponentDirectoryTree lViewDirectoryTree = new ComponentDirectoryTree( directoryInfo.Root, directoryInfo );
					
					dialogDirectorySelector = new DialogDirectorySelect( ChangeDirectory, lViewDirectoryTree, directoryInfo, directoryInfoRecentList );
				}
			}
			GUILayout.EndHorizontal();
		}

		private void ChangeDirectory( DirectoryInfo aDirectoryInfo )
		{
			if( aDirectoryInfo != null )
			{
				directoryInfo = aDirectoryInfo;
				setDirectoryInfo( directoryInfo );
			}
			
			dialogDirectorySelector = null;
		}
	}
}
