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
		
		private bool isLoop;
		private bool isMute;
		private float volume;
		private float volumeBefore;
		private float position;
		private float positionPre;

		private FileInfo fileInfo;
		private string title;

		public delegate void ChangeMusicPrevious();
		public delegate void ChangeMusicNext();
		
		public ChangeMusicPrevious changeMusicPrevious;
		public ChangeMusicNext changeMusicNext;

		public Rect Rect{ get; set; }

		public ViewPlayer( FileInfo aFileInfo, ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			fileInfo = aFileInfo;

			if( fileInfo == null )
			{
				title = "";
				player = new PlayerNull();
			}
			else
			{
				title = fileInfo.Name;
				player = LoaderCollection.LoadPlayer( fileInfo.FullName );
			}

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;
			
			isLoop = true;
			isMute = false;
			volume = 0.5f;
			volumeBefore = 0.5f;
			position = 0.0f;
			positionPre = 0.0f;
		}
		
		public void SetPlayer( FileInfo aFileInfo )
		{
			fileInfo = aFileInfo;
			
			if( fileInfo == null )
			{
				title = "";
				player = new PlayerNull();
			}
			else
			{
				title = fileInfo.Name;
				player = LoaderCollection.LoadPlayer( fileInfo.FullName );
			}

			position = 0.0f;
			positionPre = 0.0f;
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
			GUILayout.BeginVertical( GuiStyleSet.StyleGeneral.box );
			{
				GUILayout.TextArea( title, GuiStyleSet.StylePlayer.labelTitle );
				
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label( new GUIContent( player.GetTimePosition().MMSSmmm, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

					if( positionPre == position )
					{
						positionPre = ( float )player.Position;
						position = GUILayout.HorizontalScrollbar( positionPre, 0.01f, 0.0f, 1.01f, "seekbar" );
					}
					else
					{
						position = GUILayout.HorizontalScrollbar( position, 0.01f, 0.0f, 1.01f, "seekbar" );

						if( Input.GetMouseButtonUp( 0 ) == true )
						{
							player.Position = position;
							positionPre = position;
						}
					}

					GUILayout.Label( new GUIContent( player.GetTimeLength().MMSSmmm, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );
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

					bool lIsMuteAfter = GUILayout.Toggle( isMute, new GUIContent( "", "StylePlayer.ToggleMute" ), GuiStyleSet.StylePlayer.toggleMute );

					if( lIsMuteAfter != isMute )
					{
						if( lIsMuteAfter == true )
						{
							player.Volume = 0.0f;
						}
						else
						{
							player.Volume = volume;
						}

						isMute = lIsMuteAfter;
					}

					if( isMute == false )
					{
						volume = GUILayout.HorizontalSlider( volume, 0.0f, 1.00f, GuiStyleSet.StyleSlider.horizontalbar, GuiStyleSet.StyleSlider.horizontalbarThumb );
						player.Volume = volume;
					}
					else // isMute == true
					{
						float volumeAfter = GUILayout.HorizontalSlider( 0.0f, 0.0f, 1.00f, GuiStyleSet.StyleSlider.horizontalbar, GuiStyleSet.StyleSlider.horizontalbarThumb );

						if( volumeAfter != 0.0f )
						{
							isMute = false;
							volume = volumeAfter;
							player.Volume = volume;
						}
					}
					
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();				
			}
			GUILayout.EndVertical();
			
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y;
			float lY = lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top + GuiStyleSet.StylePlayer.seekbar.fixedHeight;

			isLoop = GUI.Toggle( new Rect( Screen.width / 2.0f - GuiStyleSet.StylePlayer.seekbar.fixedWidth / 2.0f, lY, 32.0f, 32.0f ), isLoop, "", GuiStyleSet.StylePlayer.toggleLoop );
			player.IsLoop = isLoop;
		}
		
		public void OnRenderObject()
		{
			//float lHeightMenu = GuiStyleSet.StyleMenu.button.CalcSize( new GUIContent( "" ) ).y;
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y;
			float lY = /*lHeightMenu +*/ lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top;

			if( player != null && player.GetTimeLength().Second != 0.0d )
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( Screen.width / 2 - lWidth / 2, lY + lHeight, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, ( float )( player.GetLoopPoint().start.Seconds / player.GetTimeLength().Seconds ), ( float )( player.GetLoopPoint().end.Seconds / player.GetTimeLength().Seconds ), ( float )player.Position );
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
			player.Update( aSoundBuffer, aChannels, aSampleRate );
		}
		
		public void OnApplicationQuit()
		{
			
		}

		public LoopInformation GetLoopPoint()
		{
			LoopInformation lLoopPoint = player.GetLoopPoint();

			if( lLoopPoint == null )
			{
				lLoopPoint = new LoopInformation( 44100, 0, 0 );
			}

			return lLoopPoint;
		}

		public FileInfo GetFileInfo()
		{
			return fileInfo;
		}
	}
}
