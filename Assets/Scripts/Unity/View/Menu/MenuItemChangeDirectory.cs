using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class MenuItemChangeDirectory : AMenuItem
	{
		private DialogDirectorySelect dialogDirectorySelector;

		public delegate void SetDirectoryInfo( DirectoryInfo aDirectoryInfo );
		private SetDirectoryInfo setDirectoryInfo;
		private List<DirectoryInfo> directoryInfoRecentList;

		public MenuItemChangeDirectory( string aTitle, SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
			: base( aTitle )
		{
			setDirectoryInfo = aSetDirectoryInfo;
			directoryInfoRecentList = aDirectoryInfoRecentList;
		}
		
		private void ChangeDirectoryInput( DirectoryInfo aDirectoryInfo )
		{
			if( aDirectoryInfo != null )
			{
				setDirectoryInfo( aDirectoryInfo );
			}
			
			dialogDirectorySelector = null;
		}

		public override void Select()
		{
			dialogDirectorySelector = new DialogDirectorySelect( ChangeDirectoryInput, directoryInfoRecentList );
		}

		public override void OnGUI()
		{
			if( dialogDirectorySelector != null )
			{
				dialogDirectorySelector.OnGUI();
			}
		}
	}
}
