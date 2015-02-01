using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.Player
{
	public class MenuBar : AMenuBar
	{
		public MenuBar( string aFilePathLanguage, MenuItemChangeDirectory.SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			MenuBoxFile lMenuBoxFile = new MenuBoxFile( lDictionaryDescription["FILE"], Application.streamingAssetsPath + "/Language/Player/Menu/MenuBar/MenuBoxFile.language", aSetDirectoryInfo, aDirectoryInfoRecentList );

			menuBoxList.Add( lMenuBoxFile );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
	}
}
