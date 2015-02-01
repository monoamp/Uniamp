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
		public MenuBoxFile( string aTitle, string aFilePathLanguage, MenuItemChangeDirectory.SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
			: base( aTitle )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			menuItemList.Add( new MenuItemChangeDirectory( lDictionaryDescription["INPUT"], aSetDirectoryInfo, aDirectoryInfoRecentList ) );
		}
	}
}
