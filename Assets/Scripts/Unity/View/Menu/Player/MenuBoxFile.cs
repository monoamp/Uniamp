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
		public MenuBoxFile( string aTitle, string aFilePathLanguage, ApplicationPlayer aApplicationPlayer )
			: base( aTitle )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			menuItemList.Add( new MenuItemChangeDirectory( lDictionaryDescription["INPUT"], aApplicationPlayer.SetInput, aApplicationPlayer.directoryInfoRecentList ) );
		}
	}
}
