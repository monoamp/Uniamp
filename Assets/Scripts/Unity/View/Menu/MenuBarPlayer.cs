using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class MenuBarPlayer : AMenuBar
	{
		public MenuBarPlayer( string aFilePathLanguage, MenuItemChangeDirectory.SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			MenuBoxFile lMenuBoxFile = new MenuBoxFile( lDictionaryDescription["FILE"], Application.streamingAssetsPath + "/Language/Player/MenuItemFile.language", aSetDirectoryInfo, aDirectoryInfoRecentList );

			menuBoxList.Add( lMenuBoxFile );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
	}
}
