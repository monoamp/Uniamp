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
	public class ComponentLoopEditor : ComponentPlayer
	{
        private IPlayer player;

		private string title;
		private bool mouseButton;

		public ChangeMusicPrevious changeMusicPrevious;
		public ChangeMusicNext changeMusicNext;

		public Rect Rect{ get; set; }

		private int positionInBuffer;
		
		private GameObject gameObjectWaveform1;
		private GameObject gameObjectWaveform2;
		private GameObject gameObjectWaveform3;

		private Transform transformWaveform1;
		private Transform transformWaveform2;
		private Transform transformWaveform3;

		private MeshFilter meshFilter1;
		private MeshFilter meshFilter2;
		private MeshFilter meshFilter3;
		
		private MeshRenderer meshRenderer1;
		private MeshRenderer meshRenderer2;
		private MeshRenderer meshRenderer3;

		private Vector3[] vertices1;
		private Vector3[] vertices2;
		private Vector3[] vertices3;
		private float[] waveform;
		
		private object objectLock;
		private ObjectWaveform objectWaveformRight;
		private ObjectWaveform objectWaveformLeft;

		public ComponentLoopEditor( ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
			: base( aChangeMusicPrevious, aChangeMusicNext )
		{
			objectLock = new object();

			mouseButton = false;

			title = "";
			player = new PlayerNull();

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;

			positionInBuffer = 0;
			
			Mesh lMesh1 = new Mesh();
			Mesh lMesh2 = new Mesh();
			Mesh lMesh3 = new Mesh();

			vertices1 = new Vector3[1281 * 2];
			int[] lIndices = new int[1281 * 2];

			for( int i = 0; i < vertices1.Length / 2; i++ )
			{
				vertices1[i * 2 + 0] = new Vector3( ( float )i / 2.0f - 640.0f, 360.0f, 0.0f );
				vertices1[i * 2 + 1] = new Vector3( ( float )i / 2.0f - 640.0f, 360.0f, 0.0f );
			}


			for( int i = 0; i < lIndices.Length; i++ )
			{
				lIndices[i] = i;
			}
			
			lMesh1.vertices = vertices1;
			lMesh1.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh1.RecalculateBounds();
			
			lMesh2.vertices = vertices1;
			lMesh2.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh2.RecalculateBounds();
			
			lMesh3.vertices = vertices1;
			lMesh3.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh3.RecalculateBounds();

			gameObjectWaveform1 = GameObject.Find( "Waveform1" );
			gameObjectWaveform2 = GameObject.Find( "Waveform2" );
			gameObjectWaveform3 = GameObject.Find( "Waveform3" );
			
			transformWaveform1 = gameObjectWaveform1.transform;
			transformWaveform2 = gameObjectWaveform2.transform;
			transformWaveform3 = gameObjectWaveform3.transform;
			
			meshFilter1 = gameObjectWaveform1.GetComponent<MeshFilter>();
			meshFilter2 = gameObjectWaveform2.GetComponent<MeshFilter>();
			meshFilter3 = gameObjectWaveform3.GetComponent<MeshFilter>();
			
			meshRenderer1 = gameObjectWaveform1.GetComponent<MeshRenderer>();
			meshRenderer2 = gameObjectWaveform2.GetComponent<MeshRenderer>();
			meshRenderer3 = gameObjectWaveform3.GetComponent<MeshRenderer>();
			
			meshFilter1.sharedMesh = lMesh1;
			meshFilter2.sharedMesh = lMesh2;
			meshFilter3.sharedMesh = lMesh3;
			
			meshFilter1.sharedMesh.name = "Waveform1";
			meshFilter2.sharedMesh.name = "Waveform2";
			meshFilter3.sharedMesh.name = "Waveform3";
			
			meshRenderer1.material.color = new Color( 0.0f, 0.0f, 1.0f, 0.5f );
			meshRenderer2.material.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
			meshRenderer3.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
			
			objectWaveformLeft = new ObjectWaveform( transformWaveform3, meshFilter3 );
			objectWaveformRight = new ObjectWaveform( transformWaveform2, meshFilter2 );
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
			MusicPcm lMusicPcm = ( MusicPcm )player.Music;
		
			if( lMusicPcm != null )
			{
				float[] lValueArray = new float[vertices1.Length];

				RiffWaveRiff lRiffWaveRiff = new RiffWaveRiff( player.GetFilePath() );
				WaveformPcm lWaveform = new WaveformPcm( lRiffWaveRiff, true );
				waveform = new float[lWaveform.format.samples];

				int lIndexPre = 0;

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
						double lY = 359.0d - 120.0d * 720.0d / Screen.height;
						
						vertices1[lIndexPre * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 0] * 30.0f * 720.0f / Screen.height ), 0.0f );
						vertices1[lIndexPre * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 1] * 30.0f * 720.0f / Screen.height ), 0.0f );
						
						lIndexPre = lIndex;
					}
				}
				
				meshFilter1.mesh.vertices = vertices1;
				meshFilter2.mesh.vertices = vertices1;
				meshFilter3.mesh.vertices = vertices1;
				meshFilter1.mesh.RecalculateBounds();
				meshFilter2.mesh.RecalculateBounds();
				meshFilter3.mesh.RecalculateBounds();
			}

			UpdateVertex();
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
			
			GuiStyleSet.StylePlayer.seekbar.fixedWidth = Screen.width;
			GuiStyleSet.StylePlayer.seekbarImage.fixedWidth = Screen.width;

			GUILayout.BeginVertical( GuiStyleSet.StylePlayer.box );
			{
				GUILayout.BeginHorizontal();
				{
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

					player.IsLoop = GUILayout.Toggle( player.IsLoop, new GUIContent( "", "StylePlayer.ToggleLoop" ), GuiStyleSet.StylePlayer.toggleLoop );
					GUILayout.Label( new GUIContent( player.GetTPosition().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

					GUILayout.Label( new GUIContent( player.GetLength().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

					GUILayout.FlexibleSpace();
					
					GUILayout.TextArea( title, GuiStyleSet.StyleGeneral.label );
				}
				GUILayout.EndHorizontal();	
				
				float lPositionFloat = ( float )player.PositionRate;
				float lPositionAfter = GUILayout.HorizontalSlider( lPositionFloat, 0.0f, 1.0f, GuiStyleSet.StylePlayer.seekbar, GuiStyleSet.StylePlayer.seekbarThumb );
				
				if( lPositionAfter != lPositionFloat )
				{
					player.PositionRate = lPositionAfter;
				}
			}
			GUILayout.EndVertical();
			
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y;
			float lY = Rect.y + lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top + GuiStyleSet.StylePlayer.seekbar.fixedHeight;
		}
		
		public void OnRenderObject()
		{
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y + GuiStyleSet.StylePlayer.labelTitle.margin.top + GuiStyleSet.StylePlayer.labelTitle.margin.bottom;
			float lY = Rect.y + lHeightTitle;

			if( player != null && player.GetLength().Second != 0.0d )
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( 0.0f, lY, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, ( float )( player.Loop.start / player.GetLength() ), ( float )( player.Loop.end / player.GetLength() ), ( float )player.PositionRate );
			}
			else
			{
				float lWidth = GuiStyleSet.StylePlayer.seekbar.fixedWidth;
				float lHeight = GuiStyleSet.StylePlayer.seekbar.fixedHeight;
				Gui.DrawSeekBar( new Rect( 0.0f, lY, lWidth, lHeight ), GuiStyleSet.StylePlayer.seekbarImage, 0.0f, 0.0f, 0.0f );
			}

			float lWidthVolume = GuiStyleSet.StylePlayer.volumebarImage.fixedWidth;
			float lHeightVolume = GuiStyleSet.StylePlayer.volumebarImage.fixedHeight;
			Gui.DrawVolumeBar( new Rect( GuiStyleSet.StylePlayer.toggleMute.fixedWidth, lHeightTitle, lWidthVolume, lHeightVolume ), GuiStyleSet.StylePlayer.volumebarImage, player.Volume );
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
		
		public override int GetLength()
		{
			return ( int )player.GetLength().sample;
		}

		public override void SetIsLoop( bool aIsLoop )
		{
			player.IsLoop = aIsLoop;
		}
		
		public override LoopInformation GetLoop()
		{
			return player.Loop;
		}

		public override void SetLoop( LoopInformation aLoopInformation )
		{
			player.SetLoop( aLoopInformation );

			UpdateVertex();
		}

		private void UpdateVertex()
		{
			objectWaveformLeft.SetLoop( player.Loop, ( int )player.GetLength().sample );
			objectWaveformRight.SetLoop( player.Loop, ( int )player.GetLength().sample );
			objectWaveformRight.SetPosition( ( player.Loop.end.sample - player.Loop.start.sample ) / player.GetLength().sample );
		}
	}
}
