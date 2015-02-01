using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View.Player
{
	public class ScenePlayer : MonoBehaviour
	{
		private int sampleRate;
		private Dictionary<int, float[]> soundBuffer;

		private ApplicationPlayer applicationPlayer;
		private bool isSetGuiStyle;

		void Awake()
		{
			isSetGuiStyle = false;

			Unity.Function.Graphic.Gui.camera = camera;
			GameObject obj = GameObject.Find( "GuiStyleSet" );
			GuiStyleSet.Reset( obj );

			AudioSettings.outputSampleRate = 44100;
			sampleRate = AudioSettings.outputSampleRate;

			applicationPlayer = new ApplicationPlayer( new DirectoryInfo( Application.streamingAssetsPath + "/Sound/Music" ) );

			soundBuffer = new Dictionary<int, float[]>();
		}

		void Start()
		{
			applicationPlayer.Start();
			
			AudioClip myClip = AudioClip.Create("MySinoid", 8820, 2, 44100, false, true, OnAudioRead, OnAudioSetPosition);
			audio.clip = myClip;
			audio.Play();

			/*
			WWW www = new WWW( "file:///" + Application.streamingAssetsPath + "/Sound/Music/tam-n17.mp3" );
			
			if( www.error != null )
			{
				Debug.Log( www.error );
			}
			
			AudioClip audioClip = www.GetAudioClip( false, false );
			audio.clip = audioClip;
*/
			/*
			float[] data = new float[audioClip.samples  * audioClip.channels];
			audioClip.GetData( data, 0 );
				
				Debug.Log( data[0].ToString() );
				Debug.Log( data[1].ToString() );*/
		}
		
		void OnAudioRead( float[] data )
		{
			for( int i = 0; i < data.Length; i++ )
			{
				data[i] = 0;
			}
			
			if( soundBuffer.ContainsKey(data.Length) == false )
			{
				soundBuffer.Add( data.Length, new float[data.Length] );
			}

			for( int i = 0; i < soundBuffer[data.Length].Length; i++ )
			{
				soundBuffer[data.Length][i] = 0.0f;
			}
					
			applicationPlayer.OnAudioFilterRead( soundBuffer[data.Length], 2, sampleRate );
					
			for( int i = 0; i < soundBuffer[data.Length].Length; i++ )
			{
				data[i] += soundBuffer[data.Length][i];
			}
		}

		void OnAudioSetPosition(int newPosition)
		{

		}

		void Update()
		{
			/*
			if( !audio.isPlaying && audio.clip.isReadyToPlay )
			{
				audio.Play();

				float[] data = new float[audio.clip.samples  * audio.clip.channels];
				audio.clip.GetData( data, 0 );
				
				Debug.Log( data[0].ToString() );
				Debug.Log( data[1].ToString() );
			}
*/
			applicationPlayer.Update();
		}

		void OnGUI()
		{
			SetGuiStyles();

			GUILayout.BeginArea( new Rect( 0.0f, 0.0f, Screen.width, Screen.height ) );
			{
				applicationPlayer.OnGUI();
			}
			GUILayout.EndArea();

			//GUI.Label( rectWindow, GUI.tooltip, GuiStyleSet.StyleGeneral.tooltip );
		}

		private void SetGuiStyles()
		{
			if( isSetGuiStyle == false )
			{
				isSetGuiStyle = true;
				
				Dictionary<string, GUIStyle> guiStyleDictionary = new Dictionary<string, GUIStyle>();
				
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.verticalbar.name, GuiStyleSet.StyleScrollbar.verticalbar );
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.verticalbarThumb.name, GuiStyleSet.StyleScrollbar.verticalbarThumb );
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.verticalbarUpButton.name, GuiStyleSet.StyleScrollbar.verticalbarUpButton );
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.verticalbarDownButton.name, GuiStyleSet.StyleScrollbar.verticalbarDownButton );
				
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.horizontalbar.name, GuiStyleSet.StyleScrollbar.horizontalbar );
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.horizontalbarThumb.name, GuiStyleSet.StyleScrollbar.horizontalbarThumb );
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.horizontalbarLeftButton.name, GuiStyleSet.StyleScrollbar.horizontalbarLeftButton );
				guiStyleDictionary.Add( GuiStyleSet.StyleScrollbar.horizontalbarRightButton.name, GuiStyleSet.StyleScrollbar.horizontalbarRightButton );
				
				guiStyleDictionary.Add( GuiStyleSet.StylePlayer.seekbar.name, GuiStyleSet.StylePlayer.seekbar );
				guiStyleDictionary.Add( GuiStyleSet.StylePlayer.seekbarThumb.name, GuiStyleSet.StylePlayer.seekbarThumb );
				guiStyleDictionary.Add( GuiStyleSet.StylePlayer.seekbarLeftButton.name, GuiStyleSet.StylePlayer.seekbarLeftButton );
				guiStyleDictionary.Add( GuiStyleSet.StylePlayer.seekbarRightButton.name, GuiStyleSet.StylePlayer.seekbarRightButton );
				
				guiStyleDictionary.Add( GuiStyleSet.StyleProgressbar.progressbar.name, GuiStyleSet.StyleProgressbar.progressbar );
				guiStyleDictionary.Add( GuiStyleSet.StyleProgressbar.progressbarThumb.name, GuiStyleSet.StyleProgressbar.progressbarThumb );
				guiStyleDictionary.Add( GuiStyleSet.StyleProgressbar.progressbarLeftButton.name, GuiStyleSet.StyleProgressbar.progressbarLeftButton );
				guiStyleDictionary.Add( GuiStyleSet.StyleProgressbar.progressbarRightButton.name, GuiStyleSet.StyleProgressbar.progressbarRightButton );
				
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.verticalbarHeader.name, GuiStyleSet.StyleTable.verticalbarHeader );
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.verticalbarHeaderThumb.name, GuiStyleSet.StyleTable.verticalbarHeaderThumb );
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.verticalbarHeaderUpButton.name, GuiStyleSet.StyleTable.verticalbarHeaderUpButton );
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.verticalbarHeaderDownButton.name, GuiStyleSet.StyleTable.verticalbarHeaderDownButton );
				
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.horizontalbarHeader.name, GuiStyleSet.StyleTable.horizontalbarHeader );
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.horizontalbarHeaderThumb.name, GuiStyleSet.StyleTable.horizontalbarHeaderThumb );
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.horizontalbarHeaderLeftButton.name, GuiStyleSet.StyleTable.horizontalbarHeaderLeftButton );
				guiStyleDictionary.Add( GuiStyleSet.StyleTable.horizontalbarHeaderRightButton.name, GuiStyleSet.StyleTable.horizontalbarHeaderRightButton );

				if( GUI.skin.GetStyle( GuiStyleSet.StylePlayer.seekbar.name ).name == "" )
				{
					GUIStyle[] lCustomStylesAfter = new GUIStyle[GUI.skin.customStyles.Length + guiStyleDictionary.Count];
					
					for( int i = 0; i < lCustomStylesAfter.Length; i++ )
					{
						lCustomStylesAfter[i] = lCustomStylesAfter[i];
					}
					
					int lIndex = 0;
					
					foreach( KeyValuePair<string, GUIStyle> l in guiStyleDictionary )
					{
						lCustomStylesAfter[GUI.skin.customStyles.Length + lIndex] = l.Value;
						lIndex++;
					}

					GUI.skin.customStyles = lCustomStylesAfter;
				}
				else
				{
					for( int i = 0; i < GUI.skin.customStyles.Length; i++ )
					{
						if( guiStyleDictionary.ContainsKey( GUI.skin.customStyles[i].name ) == true )
						{
							GUI.skin.customStyles[i] = guiStyleDictionary[GUI.skin.customStyles[i].name];
						}
					}

					GUI.skin.customStyles = GUI.skin.customStyles;
				}
			}
		}

		void OnRenderObject()
		{
			applicationPlayer.OnRenderObject();
		}
		
		void OnApplicationQuit()
		{
			applicationPlayer.OnApplicationQuit();
		}
	}
}
