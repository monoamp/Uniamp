using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class MenuLoopTool : AMenu
	{
		public MenuLoopTool( string aFilePathLanguage, ApplicationLoopTool aApplicationLoopTool )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );

			MenuBox lMenuBoxFile = new MenuBoxFileLoopTool( lDictionaryDescription["FILE"], Application.streamingAssetsPath + "/Language/LoopTool/MenuItemFile.language", aApplicationLoopTool );

			menuBoxList.Add( lMenuBoxFile );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
		
		private Dictionary<string, string> ReadDictionaryLanguage( string aFilePathLanguage )
		{
			Dictionary<string, string> lDictionaryDescription = new Dictionary<string, string>();

			try
			{
				using( StreamReader u = new StreamReader( aFilePathLanguage ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( line.IndexOf( "//" ) != 0 && line.Split( ':' ).Length == 2 )
						{
							string key = line.Split( ':' )[0];
							string description = line.Split( ':' )[1];

							lDictionaryDescription.Add( key, description );
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
			}

			return lDictionaryDescription;
		}
	}
}
