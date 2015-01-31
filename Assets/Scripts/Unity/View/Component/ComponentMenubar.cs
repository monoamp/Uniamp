using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class ComponentMenubar : IView
	{
		private MenuBoxFile menuBoxFile;
		
		public Rect Rect{ get; set; }

		public ComponentMenubar( MenuBoxFile sMenuItemFile )
		{
			menuBoxFile = sMenuItemFile;
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

		public void OnGUI()
		{
			GUILayout.BeginHorizontal( GuiStyleSet.StyleMenu.bar );
			{
				float lHeightMenu = GuiStyleSet.StyleMenu.item.CalcSize( new GUIContent( "" ) ).y;
				
				menuBoxFile.rectMenu = new Rect( GuiStyleSet.StyleMenu.button.margin.left, GuiStyleSet.StyleMenu.bar.fixedHeight, 100.0f, lHeightMenu * 2 );

				if( GUILayout.Button( new GUIContent( "File", "StyleMenu.Button" ), GuiStyleSet.StyleMenu.button ) == true )
				{
					menuBoxFile.Awake();
				}

				//float lWidthMenu = GuiStyleSet.StyleMenu.button.CalcSize( new GUIContent( "File" ) ).x + GuiStyleSet.StyleMenu.button.margin.left * 2 + GuiStyleSet.StyleMenu.button.margin.right;
				
				if( GUILayout.Button( new GUIContent( "Config", "StyleMenu.Button" ), GuiStyleSet.StyleMenu.button ) == true )
				{
					menuBoxFile.Awake();
				}

				GUILayout.Button( new GUIContent( "Help", "StyleMenu.Button" ), GuiStyleSet.StyleMenu.button );
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			menuBoxFile.OnGUI();
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
