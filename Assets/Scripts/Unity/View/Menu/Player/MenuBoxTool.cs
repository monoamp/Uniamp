using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.Player
{
	public class MenuBoxTool : AMenuBox
	{
		public MenuBoxTool( string aTitle, string aFilePathLanguage, ApplicationPlayer aApplicationPlayer )
			: base( aTitle )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			menuItemList.Add( new MenuItemSettings( lDictionaryDescription["SETTINGS"], Application.streamingAssetsPath + "/Language/Player/Dialog/DialogSettings.language", aApplicationPlayer ) );
		}
	}
}
