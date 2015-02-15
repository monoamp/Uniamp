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
	public class ComponentLoopSelector : IView
	{
		public Rect Rect{ get; set; }
		public Rect rect;

		private ComponentLoopEditor componentLoopEditor;

		private Vector2 scrollPosition;
		private LoopInformation[][] loopArrayArray;
		private PlayMusicInformation playMusicInformation;
		private int x;
		private int y;
		
		private bool isShow;

		public ComponentLoopSelector( ComponentLoopEditor aComponentPlayer )
		{
			componentLoopEditor = aComponentPlayer;
			scrollPosition = Vector2.zero;
			x = 0;
			y = 0;
			isShow = false;
			rect = new Rect( Screen.width - 340.0f, 210.0f, 320.0f, Screen.height - 230.0f );
		}

		public void SetPlayMusicInformation( PlayMusicInformation aPlayMusicInformation )
		{
			scrollPosition = Vector2.zero;
			x = 0;
			y = 0;

			playMusicInformation = aPlayMusicInformation;

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
		
		private void SelectItemWindow( int windowID )
		{
			int lX = x;
			int lY = y;

			GUILayout.BeginVertical();
			{
				GUILayout.BeginScrollView( new Vector2( scrollPosition.x, 0.0f ), false, true, GuiStyleSet.StyleTable.horizontalbarHeader, GuiStyleSet.StyleTable.verticalbarHeader, GuiStyleSet.StyleGeneral.none );
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label( new GUIContent( "Group No.", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 80.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Length (Sample)", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.MinWidth( 120.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Count", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 80.0f ) );
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				
				scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, true, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar, GuiStyleSet.StyleScrollbar.view );
				{
					if( playMusicInformation != null )
					{
						for( int i = 0; i < loopArrayArray.Length && i < 128; i++ )
						{
							GUILayout.BeginHorizontal();
							{
								GUILayout.Label( new GUIContent( ( i + 1 ).ToString(), "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label, GUILayout.Width( 80.0f ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
								
								if( i == x )
								{
									GUILayout.Toggle( true, new GUIContent( loopArrayArray[i][0].length.sample.ToString(), "StyleList.ToggleLine " ), GuiStyleSet.StyleList.toggleLine, GUILayout.MinWidth( 120.0f ) );
								}
								else
								{
									if( GUILayout.Button( new GUIContent ( loopArrayArray[i][0].length.sample.ToString(), "StyleList.ToggleLine" ), GuiStyleSet.StyleList.toggleLine, GUILayout.MinWidth( 120.0f ) ) == true )
									{
										lX = i;
										lY = 0;
									}
								}
								
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
								GUILayout.Label( new GUIContent( loopArrayArray[i].Length.ToString(), "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label, GUILayout.Width( 80.0f ) );
							}
							GUILayout.EndHorizontal();
						}
					}
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.BeginVertical( GUILayout.Width( 80.0f ) );
						{
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
						
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
						
						GUILayout.BeginVertical( GUILayout.MinWidth( 120.0f ) );
						{
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
						
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
						
						GUILayout.BeginVertical( GUILayout.Width( 80.0f ) );
						{
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
			}
			GUILayout.EndVertical();
			
			if( x != lX || y != lY )
			{
				x = lX;
				y = lY;
				
				componentLoopEditor.SetLoop( loopArrayArray[x][y] );
				playMusicInformation.loopPoint = loopArrayArray[x][y];
				playMusicInformation.music.Loop = loopArrayArray[x][y];
				playMusicInformation.isSelected = true;
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
			int lX = x;
			int lY = y;
			
			if( isShow == true )
			{
				GUI.Window( 10, rect, SelectItemWindow, "", GuiStyleSet.StyleMenu.window );
			}
			
			if( Input.GetMouseButtonDown( 0 ) == true )
			{
				float lYY = Screen.height - 1 - Input.mousePosition.y;
				
				if( Input.mousePosition.x < rect.x || Input.mousePosition.x >= rect.x + rect.width ||
				   lYY < rect.y || lYY >= rect.y + rect.height )
				{
					isShow = false;
				}
			}

			if( playMusicInformation != null )
			{
				GUILayout.BeginHorizontal();
				{
					int lLoopCountY = 1;
					lLoopCountY = loopArrayArray[x].Length;

					if( lLoopCountY == 0 )
					{
						lLoopCountY = 1;
					}
					/*
					if( GUILayout.Button( new GUIContent ( "<-", "StyleGeneral.ButtonCircle" ), GuiStyleSet.StyleGeneral.buttonCircle ) == true )
					{
						lY--;
					}*/
					
					GUILayout.BeginVertical();
					{
						GUILayout.Label( new GUIContent( "Start", "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
						GUILayout.TextField( componentLoopEditor.GetLoop().start.sample.ToString() );
					}
					GUILayout.EndVertical();
					
					GUILayout.BeginVertical();
					{
						GUILayout.Label( new GUIContent( "End", "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
						GUILayout.TextField( componentLoopEditor.GetLoop().end.sample.ToString() );
					}
					GUILayout.EndVertical();
					
					GUILayout.BeginVertical();
					{
						GUILayout.Label( new GUIContent( "Length", "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
						GUILayout.TextField( componentLoopEditor.GetLoop().length.sample.ToString() );
					}
					GUILayout.EndVertical();
		
					if( GUILayout.Button( new GUIContent( "", "StyleGeneral.ButtonPullDown" ), GuiStyleSet.StyleGeneral.buttonPullDown ) == true )
					{
						isShow = true;
					}
					/*
					if( GUILayout.Button( new GUIContent ( "->", "StyleGeneral.ButtonCircle" ), GuiStyleSet.StyleGeneral.buttonCircle ) == true )
					{
						lY++;
					}*/
					
					if( lY < 0 )
					{
						lY = 0;
					}
					
					if( lY >= lLoopCountY )
					{
						lY = lLoopCountY - 1;
					}
				}
				GUILayout.EndHorizontal();
			}

			if( x != lX || y != lY )
			{
				x = lX;
				y = lY;
				
				componentLoopEditor.SetLoop( loopArrayArray[x][y] );
				playMusicInformation.loopPoint = loopArrayArray[x][y];
				playMusicInformation.music.Loop = loopArrayArray[x][y];
				playMusicInformation.isSelected = true;
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

