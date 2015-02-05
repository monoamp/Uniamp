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
	public class ComponentPlayer : IView
	{
        private IPlayer player;

		private string title;
		private bool mouseButton;

		public delegate void ChangeMusicPrevious();
		public delegate void ChangeMusicNext();
		
		public ChangeMusicPrevious changeMusicPrevious;
		public ChangeMusicNext changeMusicNext;

		public Rect Rect{ get; set; }

		private int positionInBuffer;

		public ComponentPlayer( string aFilePath, ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			mouseButton = false;

			if( aFilePath == null )
			{
				title = "";
				player = new PlayerNull();
			}
			else
			{
				title = Path.GetFileNameWithoutExtension( aFilePath );
				player = LoaderCollection.LoadPlayer( aFilePath );
			}

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;

			positionInBuffer = 0;
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
				title = Path.GetFileNameWithoutExtension( aFilePath );
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

			GUILayout.BeginVertical( GuiStyleSet.StylePlayer.box );
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
						player.Volume = GUILayout.HorizontalSlider( player.Volume, 0.0f, 1.00f, GuiStyleSet.StylePlayer.volumebar, GuiStyleSet.StyleSlider.horizontalbarThumb );

						if( player.Volume == 0.0f )
						{
							player.IsMute = true;
						}
					}
					else // isMute == true
					{
						float lVolume = GUILayout.HorizontalSlider( 0.0f, 0.0f, 1.00f, GuiStyleSet.StylePlayer.volumebar, GuiStyleSet.StyleSlider.horizontalbarThumb );

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
			float lY = Rect.y + lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top + GuiStyleSet.StylePlayer.seekbar.fixedHeight;

			player.IsLoop = GUI.Toggle( new Rect( Screen.width / 2.0f - GuiStyleSet.StylePlayer.seekbar.fixedWidth / 2.0f, lY, 32.0f, 32.0f ), player.IsLoop, new GUIContent( "", "StylePlayer.ToggleLoop" ), GuiStyleSet.StylePlayer.toggleLoop );
		}
		
		public void OnRenderObject()
		{
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y + GuiStyleSet.StylePlayer.labelTitle.margin.top + GuiStyleSet.StylePlayer.labelTitle.margin.bottom;
			float lY = Rect.y + lHeightTitle;

			if( player != null && player.GetLength().Second != 0.0d )
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( Screen.width / 2 - lWidth / 2, lY, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, ( float )( player.Loop.start / player.GetLength() ), ( float )( player.Loop.end / player.GetLength() ), ( float )player.PositionRate );
			}
			else
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( Screen.width / 2 - lWidth / 2, lY, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, 0.0f, 0.0f, 0.0f );
			}
			
			float lYVolume = Rect.y + lHeightTitle + GuiStyleSet.StylePlayer.toggleStartPause.fixedHeight + GuiStyleSet.StylePlayer.seekbar.fixedHeight + 18;
			float lWidthVolume = GuiStyleSet.StylePlayer.volumebarImage.fixedWidth;
			float lHeightVolume = GuiStyleSet.StylePlayer.volumebarImage.fixedHeight;
			Gui.DrawVolumeBar( new Rect( Screen.width / 2 - lWidthVolume / 2, lYVolume, lWidthVolume, lHeightVolume ), GuiStyleSet.StylePlayer.volumebarImage, player.Volume );
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			positionInBuffer = player.Update( aSoundBuffer, aChannels, aSampleRate, positionInBuffer );

			int lLength = aSoundBuffer.Length / aChannels;

			if( positionInBuffer != lLength && mouseButton == false )
			{
				changeMusicNext();

				positionInBuffer = player.Update( aSoundBuffer, aChannels, aSampleRate, positionInBuffer );
			}

			positionInBuffer %= lLength;
		}
		
		public void OnApplicationQuit()
		{
			
		}

		public string GetFilePath()
		{
			return player.GetFilePath();
		}
		
		public bool GetIsLoop()
		{
			return player.IsLoop;
		}

		public void SetIsLoop( bool aIsLoop )
		{
			player.IsLoop = aIsLoop;
		}

		public void SetLoop( LoopInformation aLoopInformation )
		{
			player.SetLoop( aLoopInformation );
		}
	}
}
