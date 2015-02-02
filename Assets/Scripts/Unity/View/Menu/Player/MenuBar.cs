using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.Player
{
	public class MenuBar : AMenuBar
	{
		public MenuBar( string aFilePathLanguage, ApplicationPlayer aApplicationPlayer )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );
			
			MenuBoxFile lMenuBoxFile = new MenuBoxFile( lDictionaryDescription["FILE"], Application.streamingAssetsPath + "/Language/Player/Menu/MenuBar/MenuBoxFile.language", aApplicationPlayer );
			MenuBoxTool lMenuBoxTool = new MenuBoxTool( lDictionaryDescription["TOOL"], Application.streamingAssetsPath + "/Language/Player/Menu/MenuBar/MenuBoxTool.language", aApplicationPlayer );
			
			menuBoxList.Add( lMenuBoxFile );
			menuBoxList.Add( lMenuBoxTool );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
	}
}
