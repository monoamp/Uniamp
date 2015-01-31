using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class MenuPlayer : AMenu
	{
		public MenuPlayer( string aFilePathLanguage, DirectoryInfo aDirectoryInfo, MenuItemChangeDirectory.SetDirectoryInfo aSetDirectoryInfo, List<DirectoryInfo> aDirectoryInfoRecentList )
		{
			List<string> lListDescription = ReadListLanguage( aFilePathLanguage );

			MenuBox lMenuBoxFile = new MenuBoxFile( lListDescription[0], Application.streamingAssetsPath + "/Language/Player/MenuItemFile.language", aDirectoryInfo, aSetDirectoryInfo, aDirectoryInfoRecentList );

			menuBoxList.Add( lMenuBoxFile );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
		
		private List<string> ReadListLanguage( string aFilePathLanguage )
		{
			List<string> lListDescription = new List<string>();

			try
			{
				using( StreamReader u = new StreamReader( aFilePathLanguage ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						lListDescription.Add( line );
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
			}

			return lListDescription;
		}
	}
}
