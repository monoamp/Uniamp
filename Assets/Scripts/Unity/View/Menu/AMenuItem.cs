using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public abstract class AMenuItem
	{
		public readonly string title;
		
		public AMenuItem( string aTitle )
		{
			title = aTitle;
		}
		
		protected Dictionary<string, string> ReadDictionaryLanguage( string aFilePathLanguage )
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
				Logger.BreakError( "Exception:" + aExpection );
			}
			
			return lDictionaryDescription;
		}

		public float GetWidth()
		{
			return GuiStyleSet.StyleMenu.item.CalcSize( new GUIContent( title ) ).x;
		}

		public abstract void Select();
		public abstract void OnGUI();
	}
}
