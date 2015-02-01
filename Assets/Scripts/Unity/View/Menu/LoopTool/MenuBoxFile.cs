using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.LoopTool
{
	public class MenuBoxFile : AMenuBox
	{
		public MenuBoxFile( string aTitle, string aFilePathLanguage, ApplicationLoopTool aApplicationLoopTool )
			: base( aTitle )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );
			
			MenuItemChangeDirectory lMenuItemChangeDirectoryInput = new MenuItemChangeDirectory( lDictionaryDescription["INPUT"], aApplicationLoopTool.SetInput, aApplicationLoopTool.directoryInfoRecentInputList );
			MenuItemChangeDirectory lMenuItemChangeDirectoryOutput = new MenuItemChangeDirectory( lDictionaryDescription["OUTPUT"], aApplicationLoopTool.SetOutput, aApplicationLoopTool.directoryInfoRecentOutputList );
			
			menuItemList.Add( lMenuItemChangeDirectoryInput );
			menuItemList.Add( lMenuItemChangeDirectoryOutput );
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
