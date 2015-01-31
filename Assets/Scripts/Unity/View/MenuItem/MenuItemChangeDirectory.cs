using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class MenuItemChangeDirectory : IView
	{
		private DialogDirectorySelect dialogDirectorySelector;

		public Rect Rect{ get; set; }
		
		private DirectoryInfo directoryInfo;
		private DirectoryInfo directoryInfoRoot;
		public delegate void SetDirectoryInfo( DirectoryInfo aDirectoryInfo );
		private SetDirectoryInfo setDirectoryInfo;
		private List<DirectoryInfo> directoryInfoRecentList;

		public MenuItemChangeDirectory( DirectoryInfo aDirectoryInfoRoot, DirectoryInfo aDirectoryInfo, SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{	
			directoryInfo = aDirectoryInfo;
			directoryInfoRoot = aDirectoryInfoRoot;
			setDirectoryInfo = aSetDirectoryInfo;
			directoryInfoRecentList = aDirectoryInfoRecentList;
		}
		
		public void Select()
		{
			ComponentDirectoryTree lViewDirectoryTree = new ComponentDirectoryTree( directoryInfoRoot, directoryInfo );
			
			dialogDirectorySelector = new DialogDirectorySelect( ChangeDirectoryInput, lViewDirectoryTree, directoryInfo, directoryInfoRecentList );
		}
		
		private void ChangeDirectoryInput( DirectoryInfo aDirectoryInfo )
		{
			Debug.Log( "Change Input" );
			dialogDirectorySelector = null;
			setDirectoryInfo( aDirectoryInfo );
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

		public void OnGUI()
		{
			if( dialogDirectorySelector != null )
			{
				dialogDirectorySelector.OnGUI();
			}
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
	}
}
