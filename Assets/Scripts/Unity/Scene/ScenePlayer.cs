using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;
using Unity.View;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.Scene
{
	public class ScenePlayer : MonoBehaviour
	{
		private int sampleRate;
		private Dictionary<int, float[]> soundBuffer;

		private Dictionary<string, GUIStyle> guiStyleDictionary;

		MeshRenderer meshRenderer;
		TextMesh textMesh;
		
		public delegate void DelegateAudioFilter( float[] aSoundBuffer, int aChannels, int aSampleRate );
		public DelegateAudioFilter delegateAudioFilter;

		private ApplicationPlayer windowLoopPlayer;
		private bool isSetGuiStyle;
		
		private Rect rectWindow;

		void Awake()
		{
			isSetGuiStyle = false;
			rectWindow = new Rect( 0.0f, 0.0f, Screen.width, Screen.height );

			Unity.Function.Graphic.Gui.camera = camera;
			GameObject obj = GameObject.Find( "GuiStyleSet" );
			GuiStyleSet.Reset( obj );

			AudioSettings.outputSampleRate = 44100;
			sampleRate = AudioSettings.outputSampleRate;

			windowLoopPlayer = new ApplicationPlayer( new DirectoryInfo( Application.streamingAssetsPath + "/Sound/Music" ) );

			soundBuffer = new Dictionary<int, float[]>();

			guiStyleDictionary = new Dictionary<string, GUIStyle>();
			
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
		}

		void Start()
		{
			windowLoopPlayer.Start();
			
			AudioClip myClip = AudioClip.Create("MySinoid", 8820, 2, 44100, false, true, OnAudioRead, OnAudioSetPosition);
			audio.clip = myClip;
			audio.Play();
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
					
			windowLoopPlayer.OnAudioFilterRead( soundBuffer[data.Length], 2, sampleRate );
					
			for( int i = 0; i < soundBuffer[data.Length].Length; i++ )
			{
				data[i] += soundBuffer[data.Length][i];
			}

            if( delegateAudioFilter != null )
			{
				for( int i = 0; i < soundBuffer[data.Length].Length; i++ )
				{
					soundBuffer[data.Length][i] = 0.0f;
				}

				delegateAudioFilter( soundBuffer[data.Length], 2, sampleRate );
				
				for( int i = 0; i < soundBuffer[data.Length].Length; i++ )
				{
					data[i] += soundBuffer[data.Length][i];
				}
            }
		}

		void OnAudioSetPosition(int newPosition)
		{

		}

		void Update()
		{
			windowLoopPlayer.Update();
		}

		void OnGUI()
		{
			SetGuiStyles();

			GUILayout.BeginArea( rectWindow );
			{
				windowLoopPlayer.OnGUI();
			}
			GUILayout.EndArea();

			//GUI.Label( rectWindow, GUI.tooltip, GuiStyleSet.StyleGeneral.tooltip );
		}

		private void SetGuiStyles()
		{
			if( isSetGuiStyle == false )
			{
				isSetGuiStyle = true;

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
			windowLoopPlayer.OnRenderObject();
		}
		
		void OnApplicationQuit()
		{
			windowLoopPlayer.OnApplicationQuit();
		}
	}
}
