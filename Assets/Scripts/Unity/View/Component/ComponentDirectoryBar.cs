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
	public class ComponentDirectoryBar : IView
	{
		private DialogDirectorySelect dialogDirectorySelector;

		public delegate void SetDirectoryInfo( DirectoryInfo aDirectoryInfo );
		private SetDirectoryInfo setDirectoryInfo;
		private List<DirectoryInfo> directoryInfoRecentList;

		public Rect Rect{ get; set; }

		public ComponentDirectoryBar( SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			setDirectoryInfo = aSetDirectoryInfo;
			directoryInfoRecentList = aDirectoryInfoRecentList;
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
			if( dialogDirectorySelector != null )
			{
				dialogDirectorySelector.OnGUI();
			}
			
			float lWifthPadding = GuiStyleSet.StyleFolder.background.padding.left + GuiStyleSet.StyleFolder.background.padding.right;
			float lWifthMargin = GuiStyleSet.StyleFolder.background.margin.left + GuiStyleSet.StyleFolder.background.margin.right;
			float lWidth = GuiStyleSet.StyleFolder.buttonFolder.CalcSize( new GUIContent( "" ) ).x + lWifthPadding + lWifthMargin;

			GUILayout.BeginHorizontal( GuiStyleSet.StyleFolder.background );
			{
				GUILayout.TextArea( directoryInfoRecentList[0].FullName, GuiStyleSet.StyleFolder.text, GUILayout.Width( Screen.width / 2.0f - lWidth ) );
				
				if( GUILayout.Button( new GUIContent( "", "StyleFolder.ButtonFolder" ), GuiStyleSet.StyleFolder.buttonFolder ) == true )
				{
					dialogDirectorySelector = new DialogDirectorySelect( ChangeDirectory, directoryInfoRecentList );
					dialogDirectorySelector.Awake();
				}
			}
			GUILayout.EndHorizontal();
		}

		private void ChangeDirectory( DirectoryInfo aDirectoryInfo )
		{
			if( aDirectoryInfo != null )
			{
				setDirectoryInfo( aDirectoryInfo );
			}
			
			dialogDirectorySelector = null;
		}
	}
}
