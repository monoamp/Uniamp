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

		private FileInfo fileInfo;
		private string title;
		private bool mouseButton;

		public delegate void ChangeMusicPrevious();
		public delegate void ChangeMusicNext();
		
		public ChangeMusicPrevious changeMusicPrevious;
		public ChangeMusicNext changeMusicNext;

		public Rect Rect{ get; set; }

		public ViewPlayer( FileInfo aFileInfo, ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			fileInfo = aFileInfo;
			mouseButton = false;

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
		}
		
		public void SetPlayer( FileInfo aFileInfo )
		{
			bool lIsMute = player.IsMute;
			bool lIsLoop = player.IsLoop;
			float lVolume = player.Volume;

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
					GUILayout.Label( new GUIContent( player.GetTimePosition().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

					float lPositionFloat = ( float )player.Position;
					float lPositionAfter = GUILayout.HorizontalScrollbar( lPositionFloat, 0.01f, 0.0f, 1.01f, "seekbar" );

					if( lPositionAfter != lPositionFloat )
					{
						player.Position = lPositionAfter;
					}

					GUILayout.Label( new GUIContent( player.GetTimeLength().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );
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

			if( player != null && player.GetTimeLength().Second != 0.0d )
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( Screen.width / 2 - lWidth / 2, lY + lHeight, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, ( float )( player.Loop.start.Seconds / player.GetTimeLength().Seconds ), ( float )( player.Loop.end.Seconds / player.GetTimeLength().Seconds ), ( float )player.Position );
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

			if( player.Position >= 1.0f && mouseButton == false )
			{
				changeMusicNext();
			}
		}
		
		public void OnApplicationQuit()
		{
			
		}

		public FileInfo GetFileInfo()
		{
			return fileInfo;
		}
	}
}
