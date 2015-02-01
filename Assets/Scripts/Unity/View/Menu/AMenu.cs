using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public abstract class AMenu : IView
	{
		protected readonly List<MenuBox> menuBoxList;
		
		public Rect Rect{ get; set; }

		public AMenu()
		{
			menuBoxList = new List<MenuBox>();
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

				foreach( MenuBox l in menuBoxList )
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

			foreach( MenuBox l in menuBoxList )
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
