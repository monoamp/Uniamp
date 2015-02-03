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
		private bool isShow;

		public MenuItemChangeDirectory( string aTitle, SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
			: base( aTitle )
		{
			setDirectoryInfo = aSetDirectoryInfo;
			dialogDirectorySelector = new DialogDirectorySelect( ChangeDirectoryInput, aDirectoryInfoRecentList );
			isShow = false;
		}
		
		private void ChangeDirectoryInput( DirectoryInfo aDirectoryInfo )
		{
			if( aDirectoryInfo != null )
			{
				setDirectoryInfo( aDirectoryInfo );
			}
			
			isShow = false;
		}
		
		public override void Select()
		{
			isShow = true;
			dialogDirectorySelector.Awake();
		}

		public override void OnGUI()
		{
			if( isShow == true )
			{
				dialogDirectorySelector.OnGUI();
			}
		}
	}
}
