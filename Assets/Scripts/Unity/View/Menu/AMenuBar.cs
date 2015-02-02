using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public abstract class AMenuBar : IView
	{
		protected readonly List<AMenuBox> menuBoxList;
		
		public Rect Rect{ get; set; }

		public AMenuBar()
		{
			menuBoxList = new List<AMenuBox>();
		}

		public void Awake()
		{

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

		public void Start()
		{
			
		}
		
		public void Update()
		{
			
		}

		public void OnGUI()
		{
			GUILayout.BeginHorizontal( GuiStyleSet.StyleMenu.bar );
			{
				float lX = GuiStyleSet.StyleMenu.button.margin.left;
				float lY = GuiStyleSet.StyleMenu.bar.fixedHeight;
				float lHeightItem = GuiStyleSet.StyleMenu.item.CalcSize( new GUIContent( "" ) ).y;

				foreach( AMenuBox l in menuBoxList )
				{
					float lWidthMax = l.GetWidth();
					float lHeightBox = lHeightItem * l.GetCount() + menuBoxList.Count + GuiStyleSet.StyleMenu.window.padding.left + GuiStyleSet.StyleMenu.window.padding.right;

					l.rect = new Rect( lX, lY, lWidthMax, lHeightBox );

					if( GUILayout.Button( new GUIContent( l.title, "StyleMenu.Button" ), GuiStyleSet.StyleMenu.button ) == true )
					{
						l.Select();
					}

					lX += GuiStyleSet.StyleMenu.button.CalcSize( new GUIContent( l.title ) ).x;
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			foreach( AMenuBox l in menuBoxList )
			{
				l.OnGUI();
			}
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{

		}
		
		public void OnApplicationQuit()
		{

		}

		public void OnRenderObject()
		{

		}
	}
}
