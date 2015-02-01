using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class MenuBarLoopTool : AMenuBar
	{
		public MenuBarLoopTool( string aFilePathLanguage, ApplicationLoopTool aApplicationLoopTool )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			MenuBoxFileLoopTool lMenuBoxFile = new MenuBoxFileLoopTool( lDictionaryDescription["FILE"], Application.streamingAssetsPath + "/Language/LoopTool/MenuItemFile.language", aApplicationLoopTool );

			menuBoxList.Add( lMenuBoxFile );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
	}
}
