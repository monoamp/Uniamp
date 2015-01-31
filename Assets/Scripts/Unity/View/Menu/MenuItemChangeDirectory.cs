using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class MenuItemChangeDirectory : IMenuItem
	{
		public string Title{ get{ return title; } }
		public readonly string title;
		public Rect Rect{ get; set; }

		private DialogDirectorySelect dialogDirectorySelector;

		private DirectoryInfo directoryInfo;
		public delegate void SetDirectoryInfo( DirectoryInfo aDirectoryInfo );
		private SetDirectoryInfo setDirectoryInfo;
		private List<DirectoryInfo> directoryInfoRecentList;

		public MenuItemChangeDirectory( string aTitle, DirectoryInfo aDirectoryInfo, SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			title = aTitle;
			directoryInfo = aDirectoryInfo;
			setDirectoryInfo = aSetDirectoryInfo;
			directoryInfoRecentList = aDirectoryInfoRecentList;
		}

		public float GetWidth()
		{
			return GuiStyleSet.StyleMenu.item.CalcSize( new GUIContent( title ) ).x;
		}

		public void Select()
		{
			ComponentDirectoryTree lViewDirectoryTree = new ComponentDirectoryTree( directoryInfo.Root, directoryInfo );
			
			dialogDirectorySelector = new DialogDirectorySelect( ChangeDirectoryInput, lViewDirectoryTree, directoryInfo, directoryInfoRecentList );
		}
		
		private void ChangeDirectoryInput( DirectoryInfo aDirectoryInfo )
		{
			if( aDirectoryInfo != null )
			{
				setDirectoryInfo( aDirectoryInfo );
			}

			dialogDirectorySelector = null;
		}

		public void OnGUI()
		{
			if( dialogDirectorySelector != null )
			{
				dialogDirectorySelector.OnGUI();
			}
		}
	}
}
