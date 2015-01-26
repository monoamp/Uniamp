using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;
using Unity.Function.Graphic;

using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Component.Sound.Player;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Unity.View
{
	public class ViewPlayer : IView
	{
        private IPlayer player;

		private string title;
		private bool mouseButton;

		public delegate void ChangeMusicPrevious();
		public delegate void ChangeMusicNext();
		
		public ChangeMusicPrevious changeMusicPrevious;
		public ChangeMusicNext changeMusicNext;

		public Rect Rect{ get; set; }

		public ViewPlayer( string aFilePath, ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			mouseButton = false;

			if( aFilePath == null )
			{
				title = "";
				player = new PlayerNull();
			}
			else
			{
				title = "";//System.IO.File..Name;
				player = LoaderCollection.LoadPlayer( aFilePath );
			}

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;
		}
		
		public void SetPlayer( string aFilePath )
		{
			bool lIsMute = player.IsMute;
			bool lIsLoop = player.IsLoop;
			float lVolume = player.Volume;

			if( aFilePath == null )
			{
				title = "";
				player = new PlayerNull();
			}
			else
			{
				title = "";//name.Name;
				player = LoaderCollection.LoadPlayer( aFilePath );
			}

			player.IsMute = lIsMute;
			player.IsLoop = lIsLoop;
			player.Volume = lVolume;
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
			mouseButton = Input.GetMouseButton( 0 );

			GUILayout.BeginVertical( GuiStyleSet.StyleGeneral.box );
			{
				GUILayout.TextArea( title, GuiStyleSet.StylePlayer.labelTitle );
				
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label( new GUIContent( player.GetTPosition().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

					float lPositionFloat = ( float )player.PositionRate;
					float lPositionAfter = GUILayout.HorizontalScrollbar( lPositionFloat, 0.01f, 0.0f, 1.01f, "seekbar" );

					if( lPositionAfter != lPositionFloat )
					{
						player.PositionRate = lPositionAfter;
					}

					GUILayout.Label( new GUIContent( player.GetLength().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
	                GUILayout.FlexibleSpace();

					if( GUILayout.Button( new GUIContent( "", "StylePlayer.ButtonPrevious" ), GuiStyleSet.StylePlayer.buttonPrevious ) == true )
					{
						changeMusicPrevious();
					}

					bool lIsPlaying = GUILayout.Toggle( player.GetFlagPlaying(), new GUIContent( "", "StylePlayer.ToggleStartPause" ), GuiStyleSet.StylePlayer.toggleStartPause );

					if( lIsPlaying != player.GetFlagPlaying() )
					{
						if( lIsPlaying == true )
						{
							player.Play();
						}
						else
						{
							player.Pause();
						}
					}

					if( GUILayout.Button( new GUIContent( "", "StylePlayer.ButtonNext" ), GuiStyleSet.StylePlayer.buttonNext ) == true )
					{
						changeMusicNext();
					}
	                
	                GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();

					player.IsMute = GUILayout.Toggle( player.IsMute, new GUIContent( "", "StylePlayer.ToggleMute" ), GuiStyleSet.StylePlayer.toggleMute );

					if( player.IsMute == false )
					{
						player.Volume = GUILayout.HorizontalSlider( player.Volume, 0.0f, 1.00f, GuiStyleSet.StyleSlider.horizontalbar, GuiStyleSet.StyleSlider.horizontalbarThumb );

						if( player.Volume == 0.0f )
						{
							player.IsMute = true;
						}
					}
					else // isMute == true
					{
						float lVolume = GUILayout.HorizontalSlider( 0.0f, 0.0f, 1.00f, GuiStyleSet.StyleSlider.horizontalbar, GuiStyleSet.StyleSlider.horizontalbarThumb );

						if( lVolume != 0.0f )
						{
							player.IsMute = false;
							player.Volume = lVolume;
						}
					}
					
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();				
			}
			GUILayout.EndVertical();
			
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y;
			float lY = lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top + GuiStyleSet.StylePlayer.seekbar.fixedHeight;

			player.IsLoop = GUI.Toggle( new Rect( Screen.width / 2.0f - GuiStyleSet.StylePlayer.seekbar.fixedWidth / 2.0f, lY, 32.0f, 32.0f ), player.IsLoop, "", GuiStyleSet.StylePlayer.toggleLoop );
		}
		
		public void OnRenderObject()
		{
			//float lHeightMenu = GuiStyleSet.StyleMenu.button.CalcSize( new GUIContent( "" ) ).y;
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y;
			float lY = /*lHeightMenu +*/ lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top;

			if( player != null && player.GetLength().Second != 0.0d )
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( Screen.width / 2 - lWidth / 2, lY + lHeight, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, ( float )( player.Loop.start.Seconds / player.GetLength().Seconds ), ( float )( player.Loop.end.Seconds / player.GetLength().Seconds ), ( float )player.PositionRate );
			}
			else
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( Screen.width / 2 - lWidth / 2, lY + lHeight, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, 0.0f, 0.0f, 0.0f );
			}
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			int lPositionEnd = player.Update( aSoundBuffer, aChannels, aSampleRate );

			if( lPositionEnd != aSoundBuffer.Length / aChannels && mouseButton == false )
			{
				changeMusicNext();
			}
		}
		
		public void OnApplicationQuit()
		{
			
		}

		public string GetFilePath()
		{
			return player.GetFilePath();
		}
	}
}
