using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;
using Unity.Function.Graphic;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Monoamp.Common.Component.Sound.Player;
using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Data.Application.Waveform;
using Monoamp.Common.Data.Standard.Riff.Wave;
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
		
		private GameObject gameObjectWaveform1;
		private GameObject gameObjectWaveform2;

		private MeshFilter meshFilter1;
		private MeshFilter meshFilter2;
		
		private MeshRenderer meshRenderer1;
		private MeshRenderer meshRenderer2;

		private bool isFinish;
		private Vector3[] vertices;
		private float[] waveform;
		private string filePath;

		public ComponentPlayer( ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			mouseButton = false;
			isFinish = false;

			title = "";
			player = new PlayerNull();

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;

			positionInBuffer = 0;
			
			Mesh lMesh1 = new Mesh();
			Mesh lMesh2 = new Mesh();
			vertices = new Vector3[1281 * 2];
			//vertices2 = new Vector3[1281 * 2];
			int[] lIndices = new int[1281 * 2];

			for( int i = 0; i < vertices.Length / 2; i++ )
			{
				vertices[i * 2 + 0] = new Vector3( ( float )i / 2.0f - 640.0f, 360.0f, 0.0f );
				vertices[i * 2 + 1] = new Vector3( ( float )i / 2.0f - 640.0f, 360.0f, 0.0f );
				
				//vertices2[i * 2 + 0] = new Vector3( ( float )i / 2.0f - 640.0f, 360.0f, 0.0f );
				//vertices2[i * 2 + 1] = new Vector3( ( float )i / 2.0f - 640.0f, 360.0f, 0.0f );
			}
			
			for( int i = 0; i < lIndices.Length; i++ )
			{
				lIndices[i] = i;
			}
			
			lMesh1.vertices = vertices;
			lMesh1.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh1.RecalculateBounds();
			
			lMesh2.vertices = vertices;
			lMesh2.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh2.RecalculateBounds();

			gameObjectWaveform1 = GameObject.Find( "Waveform1" );
			gameObjectWaveform2 = GameObject.Find( "Waveform2" );
			
			meshFilter1 = gameObjectWaveform1.GetComponent<MeshFilter>();
			meshFilter2 = gameObjectWaveform2.GetComponent<MeshFilter>();
			
			meshRenderer1 = gameObjectWaveform1.GetComponent<MeshRenderer>();
			meshRenderer2 = gameObjectWaveform2.GetComponent<MeshRenderer>();
			
			meshFilter1.sharedMesh = lMesh1;
			meshFilter2.sharedMesh = lMesh2;
			
			meshFilter1.sharedMesh.name = "Waveform1";
			meshFilter2.sharedMesh.name = "Waveform2";
			
			meshRenderer1.material.color = new Color( 0.4f, 0.4f, 0.9f );
			meshRenderer2.material.color = new Color( 0.4f, 0.4f, 0.9f );

			filePath = "";
		}
		
		public void SetPlayer( string aFilePath )
		{
			bool lIsMute = player.IsMute;
			bool lIsLoop = player.IsLoop;
			float lVolume = player.Volume;

			title = Path.GetFileNameWithoutExtension( aFilePath );
			player = ConstructorCollection.LoadPlayer( aFilePath );

			player.IsMute = lIsMute;
			player.IsLoop = lIsLoop;
			player.Volume = lVolume;
		}

		public void UpdateMesh()
		{
			double lBase = ( player.Loop.end.sample - player.Loop.start.sample ) / player.GetLength().sample * 1280.0d;
			gameObjectWaveform2.transform.position = new Vector3( ( float )lBase, -100.0f, 0.0f );
			BeginAsyncWork( Callback );
		}

		private void BeginAsyncWork(AsyncCallback callback)
		{
			Action async = AsyncWork;
			async.BeginInvoke(callback, null);
		}

		private void AsyncWork()
		{
			MusicPcm lMusicPcm = ( MusicPcm )player.Music;
			
			if( lMusicPcm != null )
			{
				if( player.GetFilePath() != filePath )
				{
					filePath = player.GetFilePath();

					RiffWaveRiff lRiffWaveRiff = new RiffWaveRiff( filePath );
					WaveformPcm lWaveform = new WaveformPcm( lRiffWaveRiff, true );
					waveform = new float[lWaveform.format.samples];

					int lIndexPre = 0;
					
					float[] lValueArray = new float[vertices.Length];
					
					for( int i = 0; i < lValueArray.Length; i++ )
					{
						lValueArray[i] = 0.0f;
					}

					for( int i = 0; i < lWaveform.format.samples; i++ )
					{
						waveform[i] = lWaveform.data.GetSample( 0, i );
						
						int lIndex = ( int )( ( float )i / waveform.Length * Screen.width );
						float lValue = waveform[i];
						
						if( lValue > lValueArray[lIndex * 2 + 0] )
						{
							lValueArray[lIndex * 2 + 0] = lValue;
						}
						else if( lValue < lValueArray[lIndex * 2 + 1] )
						{
							lValueArray[lIndex * 2 + 1] = lValue;
						}

						if( lIndex != lIndexPre )
						{
							double lX = -640.0d + ( double )lIndexPre * 1280.0d / ( double )Screen.width;
							double lY = 359.0d - 50.0d * 720.0d / Screen.height;
							
							vertices[lIndexPre * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 0] * 30.0f * 720.0f / Screen.height ), 0.0f );
							vertices[lIndexPre * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 1] * 30.0f * 720.0f / Screen.height ), 0.0f );
							
							lIndexPre = lIndex;

							isFinish = true;
						}
					}
				}
			}
		}

		private void Callback(IAsyncResult r)
		{
			isFinish = true;
		}

		public void Awake()
		{

		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			if( isFinish == true )
			{
				isFinish = false;
				meshFilter1.mesh.vertices = vertices;
				meshFilter2.mesh.vertices = vertices;
				meshFilter1.mesh.RecalculateBounds();
				meshFilter2.mesh.RecalculateBounds();
			}
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

			BeginAsyncWork( Callback );
			
			double lBase = ( player.Loop.end.sample - player.Loop.start.sample ) / player.GetLength().sample * 1280.0d;
			gameObjectWaveform2.transform.position = new Vector3( ( float )lBase, -100.0f, 0.0f );
		}
	}
}
