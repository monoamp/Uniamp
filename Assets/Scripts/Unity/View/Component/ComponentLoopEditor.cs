using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Component.Sound;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ComponentLoopEditor : IView
	{
		public Rect Rect{ get; set; }

		private ComponentPlayer componentPlayer;

		private Vector2 scrollPosition;
		private LoopInformation[][] loopArrayArray;
		private PlayMusicInformation playMusicInformation;
		private int x;
		private int y;

		public ComponentLoopEditor( ComponentPlayer aComponentPlayer )
		{
			componentPlayer = aComponentPlayer;
			scrollPosition = Vector2.zero;
			x = 0;
			y = 0;
		}

		public void SetPlayMusicInformation( PlayMusicInformation lPlayMusicInformation )
		{
			scrollPosition = Vector2.zero;
			x = 0;
			y = 0;

			playMusicInformation = lPlayMusicInformation;

			if( playMusicInformation != null )
			{
				loopArrayArray = new LoopInformation[playMusicInformation.music.GetCountLoopX()][];
				
				for( int i = 0; i < playMusicInformation.music.GetCountLoopX(); i++ )
				{
					loopArrayArray[i] = new LoopInformation[playMusicInformation.music.GetCountLoopY( i )];
					
					for( int j = 0; j < playMusicInformation.music.GetCountLoopY( i ); j++ )
					{
						loopArrayArray[i][j] = playMusicInformation.music.GetLoop( i, j );
					}
				}
			}
			else
			{
				loopArrayArray = null;
			}
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
			if( playMusicInformation != null )
			{
				GUILayout.BeginHorizontal();
				{
					int lLoopY = 1;
					lLoopY = loopArrayArray[x].Length;
					
					if( lLoopY == 0 )
					{
						lLoopY = 1;
					}
					
					int yPre = ( y - 1 + lLoopY ) % lLoopY;
					int yNext = ( y + 1 ) % lLoopY;
					
					if( GUILayout.Button( new GUIContent ( ( yPre + 1 ).ToString() + "<-", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
					{
						y = yPre;
						componentPlayer.SetLoop( loopArrayArray[x][y] );
						playMusicInformation.loopPoint = loopArrayArray[x][y];
						playMusicInformation.music.Loop = loopArrayArray[x][y];
					}
					
					GUILayout.Label( new GUIContent ( playMusicInformation.music.GetLoop( x, y ).length.sample.ToString(), "StyleGeneral.LabelCenter" ), GuiStyleSet.StyleGeneral.labelCenter );
					
					if( GUILayout.Button( new GUIContent ( "->" + ( yNext + 1 ).ToString(), "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
					{
						y = yNext;
						componentPlayer.SetLoop( loopArrayArray[x][y] );
						playMusicInformation.loopPoint = loopArrayArray[x][y];
						playMusicInformation.music.Loop = loopArrayArray[x][y];
					}
				}
				GUILayout.EndHorizontal();
				
				scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, false, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar, GuiStyleSet.StyleScrollbar.view );
				{
					for( int i = 0; i < loopArrayArray.Length && i < 128; i++ )
					{
						if( GUILayout.Button( new GUIContent ( loopArrayArray[i][0].length.sample.ToString(), "StyleList.ToggleLine" ), GuiStyleSet.StyleList.toggleLine ) == true )
						{
							x = i;
							y = 0;
							componentPlayer.SetLoop( loopArrayArray[x][y] );
							playMusicInformation.loopPoint = loopArrayArray[x][y];
							playMusicInformation.music.Loop = loopArrayArray[x][y];
						}
					}
				}
				GUILayout.EndScrollView();
			}
		}

		public void OnRenderObject()
		{
			
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			
		}
		
		public void OnApplicationQuit()
		{
			
		}
    }
}

