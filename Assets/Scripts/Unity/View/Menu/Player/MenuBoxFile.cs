using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.Player
{
	public class MenuBoxFile : AMenuBox
	{
		private List<string> descriptionMenuItemList;
		
		public MenuBoxFile( string aTitle, string aFilePathLanguage, MenuItemChangeDirectory.SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
			: base( aTitle )
		{
			descriptionMenuItemList = new List<string>();
			ReadLanguageList( aFilePathLanguage );
			
			foreach( string l in descriptionMenuItemList )
			{
				menuItemList.Add( new MenuItemChangeDirectory( l, aSetDirectoryInfo, aDirectoryInfoRecentList ) );
			}
		}
		
		private void ReadLanguageList( string aFilePathLanguage )
		{
			try
			{
				using( StreamReader u = new StreamReader( aFilePathLanguage ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						descriptionMenuItemList.Add( line );
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
			}
		}
	}
}