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

		private ComponentPlayer componentPlayer;

		private Vector2 scrollPosition;
		private LoopInformation[][] loopArrayArray;
		private PlayMusicInformation playMusicInformation;
		private int x;
		private int y;

		public ComponentLoopSelector( ComponentPlayer aComponentPlayer )
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
			int lX = x;
			int lY = y;
			
			float positionStart = ( float )componentPlayer.GetLoop().start.sample;
			
			if( positionStart < 0.0f )
			{
				positionStart = 0.0f;
			}

			float lPositionStartPre = positionStart;
			float lLoopLength = ( float )componentPlayer.GetLoop().length.sample;

			GUILayout.BeginVertical();
			{
				GUILayout.BeginVertical();
				{
					if( playMusicInformation != null )
					{
						positionStart = GUILayout.HorizontalSlider( positionStart, 0.0f, componentPlayer.GetLength(), GUILayout.Width( Screen.width ));
				
						if( positionStart + lLoopLength > componentPlayer.GetLength() + 2 )
						{
							positionStart = componentPlayer.GetLength() - lLoopLength + 2;
						}

						GUILayout.BeginHorizontal();
						{
							int lLoopCountY = 1;
							lLoopCountY = loopArrayArray[x].Length;

							if( lLoopCountY == 0 )
							{
								lLoopCountY = 1;
							}
							
							if( GUILayout.Button( new GUIContent ( "<-", "StyleGeneral.Button" ) ) == true )
							{
								lY--;
							}

							GUILayout.BeginVertical();
							{
								GUILayout.Label( new GUIContent( "Start", "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
								GUILayout.Label( new GUIContent( loopArrayArray[x][y].start.sample.ToString(), "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
							}
							GUILayout.EndVertical();

							GUILayout.BeginVertical();
							{
								GUILayout.Label( new GUIContent( "End", "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
								GUILayout.Label( new GUIContent( loopArrayArray[x][y].end.sample.ToString(), "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label );
							}
							GUILayout.EndVertical();
							
							
							if( GUILayout.Button( new GUIContent ( "->", "StyleGeneral.Button" ) ) == true )
							{
								lY++;
							}
							
							if( lY < 0 )
							{
								lY = 0;
							}
							
							if( lY >= lLoopCountY )
							{
								lY = lLoopCountY - 1;
							}

							GUILayout.BeginVertical();
							{
								GUILayout.BeginScrollView( new Vector2( scrollPosition.x, 0.0f ), false, true, GuiStyleSet.StyleTable.horizontalbarHeader, GuiStyleSet.StyleTable.verticalbarHeader, GuiStyleSet.StyleGeneral.none );
								{
									GUILayout.BeginHorizontal();
									{
										GUILayout.Label( new GUIContent( "Group No.", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 80.0f ) );
										GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
										GUILayout.Label( new GUIContent( "Length (Sample)", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 120.0f ) );
										GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
										GUILayout.Label( new GUIContent( "Count", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 60.0f ) );
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
													GUILayout.Toggle( true, new GUIContent( loopArrayArray[i][0].length.sample.ToString(), "StyleList.ToggleLine " ), GuiStyleSet.StyleList.toggleLine, GUILayout.Width( 120.0f ) );
												}
												else
												{
													if( GUILayout.Button( new GUIContent ( loopArrayArray[i][0].length.sample.ToString(), "StyleList.ToggleLine" ), GuiStyleSet.StyleList.toggleLine, GUILayout.Width( 120.0f ) ) == true )
													{
														lX = i;
														lY = 0;
													}
												}
												
												GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
												GUILayout.Label( new GUIContent( loopArrayArray[i].Length.ToString(), "StyleGeneral.Label" ), GuiStyleSet.StyleGeneral.label, GUILayout.Width( 60.0f ) );
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
										
										GUILayout.BeginVertical( GUILayout.Width( 120.0f ) );
										{
											GUILayout.FlexibleSpace();
										}
										GUILayout.EndVertical();
										
										GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
										
										GUILayout.BeginVertical( GUILayout.Width( 60.0f ) );
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
						}
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();

			if( x != lX || y != lY )
			{
				x = lX;
				y = lY;
				
				componentPlayer.SetLoop( loopArrayArray[x][y] );
				playMusicInformation.loopPoint = loopArrayArray[x][y];
				playMusicInformation.music.Loop = loopArrayArray[x][y];
				playMusicInformation.isSelected = true;
			}

			if( positionStart != lPositionStartPre )
			{
				double lSampleRate = componentPlayer.GetLoop().length.sampleRate;
				componentPlayer.SetLoop( new LoopInformation( lSampleRate, ( int )positionStart, ( int )positionStart + ( int )lLoopLength - 1 ) );
				playMusicInformation.loopPoint = componentPlayer.GetLoop();
				playMusicInformation.music.Loop = componentPlayer.GetLoop();
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

