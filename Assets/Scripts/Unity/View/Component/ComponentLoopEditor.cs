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
		private bool mouseButtonPrevious;
		
		private readonly ComponentLoopSelector componentLoopSelector;

		public ChangeMusicPrevious changeMusicPrevious;
		public ChangeMusicNext changeMusicNext;

		public Rect Rect{ get; set; }

		private int positionInBuffer;

		private MeshFilter meshFilter;
		private MeshFilter meshFilter2;
		private MeshFilter meshFilter3;
		
		private MeshRenderer meshRenderer;
		private MeshRenderer meshRenderer2;
		private MeshRenderer meshRenderer3;

		private float[] waveform;
		
		private object objectLock;
		private ObjectWaveform objectWaveformRight;
		private ObjectWaveform objectWaveformLeft;

		private bool isOnFrameLoop;
		private Vector2 positionMousePrevious;

		public ComponentLoopEditor( ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
			: base( aChangeMusicPrevious, aChangeMusicNext )
		{
			objectLock = new object();

			mouseButtonPrevious = false;
			isOnFrameLoop = false;
			positionMousePrevious = Vector2.zero;

			title = "";
			player = new PlayerNull();

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;

			positionInBuffer = 0;
			
			Mesh lMesh = new Mesh();
			Mesh lMesh2 = new Mesh();
			Mesh lMesh3 = new Mesh();

			Vector3[] vertices = new Vector3[1281 * 2];
			int[] lIndices = new int[1281 * 2];

			for( int i = 0; i < lIndices.Length; i++ )
			{
				lIndices[i] = i;
			}
			
			lMesh.vertices = vertices;
			lMesh.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh.RecalculateBounds();
			
			lMesh2.vertices = vertices;
			lMesh2.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh2.RecalculateBounds();
			
			lMesh3.vertices = vertices;
			lMesh3.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh3.RecalculateBounds();

			GameObject gameObjectWaveform1 = GameObject.Find( "Waveform1" );
			GameObject gameObjectWaveform2 = GameObject.Find( "Waveform2" );
			GameObject gameObjectWaveform3 = GameObject.Find( "Waveform3" );

			meshFilter = gameObjectWaveform1.GetComponent<MeshFilter>();
			meshFilter2 = gameObjectWaveform2.GetComponent<MeshFilter>();
			meshFilter3 = gameObjectWaveform3.GetComponent<MeshFilter>();

			meshFilter.sharedMesh = lMesh;
			meshFilter2.sharedMesh = lMesh2;
			meshFilter3.sharedMesh = lMesh3;
			
			meshFilter.sharedMesh.name = "Waveform1";
			meshFilter2.sharedMesh.name = "Waveform2";
			meshFilter3.sharedMesh.name = "Waveform3";
			
			meshRenderer = gameObjectWaveform1.GetComponent<MeshRenderer>();
			meshRenderer2 = gameObjectWaveform2.GetComponent<MeshRenderer>();
			meshRenderer3 = gameObjectWaveform3.GetComponent<MeshRenderer>();

			meshRenderer.material.color = new Color( 0.0f, 0.0f, 1.0f, 0.5f );
			meshRenderer2.material.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
			meshRenderer3.material.color = new Color( 0.0f, 0.1f, 0.0f, 0.5f );
			
			objectWaveformLeft = new ObjectWaveform( gameObjectWaveform3.transform, meshFilter3 );
			objectWaveformRight = new ObjectWaveform( gameObjectWaveform2.transform, meshFilter2 );

			componentLoopSelector = new ComponentLoopSelector( this );
		}
		
		public void SetPlayer( string aFilePath, Dictionary<string, PlayMusicInformation> aMusicInformationDictionary )
		{
			bool lIsMute = player.IsMute;
			bool lIsLoop = player.IsLoop;
			float lVolume = player.Volume;

			title = Path.GetFileNameWithoutExtension( aFilePath );
			player = ConstructorCollection.LoadPlayer( aFilePath );

			player.IsMute = lIsMute;
			player.IsLoop = lIsLoop;
			player.Volume = lVolume;
			
			if( aMusicInformationDictionary.ContainsKey( aFilePath ) == true )
			{
				componentLoopSelector.SetPlayMusicInformation( aMusicInformationDictionary[aFilePath] );
			}
			else
			{
				componentLoopSelector.SetPlayMusicInformation( null );
			}
		}

		public void UpdateMesh()
		{
			MusicPcm lMusicPcm = ( MusicPcm )player.Music;
		
			if( lMusicPcm != null )
			{
				Vector3[] vertices = meshFilter.mesh.vertices;
				float[] lValueArray = new float[vertices.Length];

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
						double lX = -Screen.width / 2.0d + ( double )lIndexPre * ( double )Screen.width / ( ( double )Screen.width + 1.0d );
						double lY = Screen.height / 2.0d - 1.0d - 90.0d;
						
						vertices[lIndexPre * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 0] * 30.0f ), 0.0f );
						vertices[lIndexPre * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 1] * 30.0f ), 0.0f );
						
						lIndexPre = lIndex;
					}
				}
				
				meshFilter.mesh.vertices = vertices;
				meshFilter3.mesh.vertices = vertices;
				meshFilter.mesh.RecalculateBounds();
				meshFilter3.mesh.RecalculateBounds();
				
				ChangeLoop( player.Loop );
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
			if( Input.GetMouseButton( 0 ) != mouseButtonPrevious )
			{
				if( Input.GetMouseButton( 0 ) == true )
				{
					Rect lFrame = new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width ), 90.0f - 30.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width ), 60.0f );

					if( lFrame.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoop = true;
						meshRenderer3.material.color = new Color( 0.0f, 0.5f, 0.5f, 0.5f );
					}
				}
				else
				{
					isOnFrameLoop = false;
					meshRenderer3.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
				}
			}

			if( Input.GetMouseButton( 0 ) == true && isOnFrameLoop == true )
			{
				double lPositionStart = player.Loop.start.sample;

				lPositionStart += ( Event.current.mousePosition.x - positionMousePrevious.x ) * player.GetLength().sample / Screen.width;
				
				if( lPositionStart < 0.0f )
				{
					lPositionStart = 0.0f;
				}

				if( lPositionStart + player.Loop.length.sample > player.GetLength().sample + 2 )
				{
					lPositionStart = player.GetLength().sample - player.Loop.length.sample + 2;
				}

				if( lPositionStart != player.Loop.start.sample )
				{
					SetLoop( new LoopInformation( player.Loop.length.sampleRate, ( int )lPositionStart, ( int )lPositionStart + ( int )player.Loop.length.sample - 1 ) );
					//playMusicInformation.loopPoint = componentPlayer.GetLoop();
					//playMusicInformation.music.Loop = componentPlayer.GetLoop();
					//playMusicInformation.isSelected = true;
				}
			}

			mouseButtonPrevious = Input.GetMouseButton( 0 );
			positionMousePrevious = Event.current.mousePosition;
			
			GuiStyleSet.StylePlayer.seekbar.fixedWidth = Screen.width;
			GuiStyleSet.StylePlayer.seekbarImage.fixedWidth = Screen.width;
			
			GUILayout.BeginVertical( GuiStyleSet.StylePlayer.box );
			{
				GUILayout.TextArea( title, GuiStyleSet.StylePlayer.labelTitle );
				GUILayout.Label( new GUIContent( "", "StyleGeneral.None" ), GuiStyleSet.StyleGeneral.none, GUILayout.Height( 60.0f ) );
				
				float lPositionFloat = ( float )player.PositionRate;
				float lPositionAfter = GUILayout.HorizontalSlider( lPositionFloat, 0.0f, 1.0f, GuiStyleSet.StylePlayer.seekbar, GuiStyleSet.StylePlayer.seekbarThumb );
				
				if( lPositionAfter != lPositionFloat )
				{
					player.PositionRate = lPositionAfter;
				}

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
					
					GUILayout.FlexibleSpace();

					player.IsLoop = GUILayout.Toggle( player.IsLoop, new GUIContent( "", "StylePlayer.ToggleLoop" ), GuiStyleSet.StylePlayer.toggleLoop );
					GUILayout.Label( new GUIContent( player.GetTPosition().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

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

					GUILayout.Label( new GUIContent( player.GetLength().MMSS, "StylePlayer.LabelTime" ), GuiStyleSet.StylePlayer.labelTime );

					componentLoopSelector.OnGUI();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		
		public void OnRenderObject()
		{
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y + GuiStyleSet.StylePlayer.labelTitle.margin.top + GuiStyleSet.StylePlayer.labelTitle.margin.bottom;
			float lY = Rect.y + lHeightTitle + 60.0f;

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
			Gui.DrawVolumeBar( new Rect( GuiStyleSet.StylePlayer.toggleMute.fixedWidth, lY + GuiStyleSet.StylePlayer.seekbar.fixedHeight + 20.0f, lWidthVolume, lHeightVolume ), GuiStyleSet.StylePlayer.volumebarImage, player.Volume );
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			positionInBuffer = player.Update( aSoundBuffer, aChannels, aSampleRate, positionInBuffer );

			int lLength = aSoundBuffer.Length / aChannels;

			if( positionInBuffer != lLength && mouseButtonPrevious == false )
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
			if( aLoopInformation.length.sample != player.Loop.length.sample )
			{
				ChangeLoop( aLoopInformation );
			}

			player.SetLoop( aLoopInformation );
			UpdateVertex();
		}

		private void UpdateVertex()
		{
			objectWaveformLeft.SetLoop( player.Loop, ( int )player.GetLength().sample );
			objectWaveformRight.SetLoop( player.Loop, ( int )player.GetLength().sample );
		}

		private void ChangeLoop( LoopInformation aLoopInformation )
		{
			Vector3[] vertices = meshFilter2.mesh.vertices;
			float[] lValueArray = new float[vertices.Length];
			
			int lIndexPre = 0;
			
			for( int i = 0; i < lValueArray.Length; i++ )
			{
				lValueArray[i] = 0.0f;
			}

			int len = waveform.Length / Screen.width;
			int diff = ( int )aLoopInformation.length.sample % len;

			for( int i = diff; i < waveform.Length; i++ )
			{
				int lIndex = ( int )( ( float )i / waveform.Length * Screen.width );
				float lValue = waveform[i - diff];
				
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
					double lX = -Screen.width / 2.0d + ( double )lIndexPre * ( double )Screen.width / ( ( double )Screen.width + 1.0d );
					double lY = Screen.height / 2.0d - 1.0d - 90.0d;
					
					vertices[lIndexPre * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 0] * 30.0f ), 0.0f );
					vertices[lIndexPre * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + lValueArray[lIndexPre * 2 + 1] * 30.0f ), 0.0f );
					
					lIndexPre = lIndex;
				}
			}
			
			meshFilter2.mesh.vertices = vertices;
			meshFilter2.mesh.RecalculateBounds();
			
			objectWaveformRight.SetPosition( ( aLoopInformation.length.sample ) / player.GetLength().sample );
		}
	}
}
