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
		private MenuBox[] menuBoxArray;
		
		public Rect Rect{ get; set; }

		public ComponentMenubar( MenuBox[] aMenuBoxArray )
		{
			menuBoxArray = aMenuBoxArray;
		}

		public void Select()
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
				float lX = GuiStyleSet.StyleMenu.button.margin.left;
				float lY = GuiStyleSet.StyleMenu.bar.fixedHeight;
				float lHeightItem = GuiStyleSet.StyleMenu.item.CalcSize( new GUIContent( "" ) ).y;
				
				foreach( MenuBox l in menuBoxArray )
				{
					l.rectMenu = new Rect( lX, lY, 100.0f, lHeightItem * 2 );

					if( GUILayout.Button( new GUIContent( l.title, "StyleMenu.Button" ), GuiStyleSet.StyleMenu.button ) == true )
					{
						l.Select();
					}

					lX += GuiStyleSet.StyleMenu.button.CalcSize( new GUIContent( l.title ) ).x;
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();

			foreach( MenuBox l in menuBoxArray )
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
