using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.LoopTool
{
	public class MenuBar : AMenuBar
	{
		public MenuBar( string aFilePathLanguage, ApplicationLoopTool aApplicationLoopTool )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			MenuBoxFile lMenuBoxFile = new MenuBoxFile( lDictionaryDescription["FILE"], Application.streamingAssetsPath + "/Language/LoopTool/Menu/MenuBar/MenuBoxFile.language", aApplicationLoopTool );

			menuBoxList.Add( lMenuBoxFile );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
	}
}
