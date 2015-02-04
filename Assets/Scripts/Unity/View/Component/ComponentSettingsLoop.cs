using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class ComponentSettingsLoop : IView
	{
		public delegate void CloseWindow( int aConfig );
		private CloseWindow closeWindow;
        
		private string description;
        private string[] captions;
        private int grid;
		
		public Rect Rect{ get; set; }

		public ComponentSettingsLoop( CloseWindow aCloseWindow, Dictionary<string, string> aLanguage )
		{
			closeWindow = aCloseWindow;
            
			description = aLanguage["CUT"];
			captions = new string[2] { aLanguage["ON"], aLanguage["OFF"] };
            grid = 0;
		}

		public void Awake()
		{
			
		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			
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

		public void OnGUI()
		{
			GUILayout.BeginVertical();
            {
				GUILayout.Label( new GUIContent( description, "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.labelCaption );
				grid = GUILayout.SelectionGrid( grid, captions, 1, GuiStyleSet.StyleGeneral.toggleRadio );
				
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					
					if( GUILayout.Button( new GUIContent( "OK", "StyleGeneral.Button " ), GuiStyleSet.StyleGeneral.button ) == true )
					{
						closeWindow( grid );
					}
					if( GUILayout.Button( new GUIContent( "Cancel", "StyleGeneral.Button " ), GuiStyleSet.StyleGeneral.button ) == true )
					{
						closeWindow( grid );
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
	}
}
