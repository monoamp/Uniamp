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
using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Unity.View
{
	public class ComponentLoopEditor
	{
        private IPlayer player;

		private string title;
		private bool mouseButtonPrevious;
		
		private readonly ComponentLoopSelector componentLoopSelector;
		
		public delegate void ChangeMusicPrevious();
		public delegate void ChangeMusicNext();

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

		private sbyte[] waveform;
		
		private object objectLock;
		private ObjectWaveform objectWaveformRight;
		private ObjectWaveform objectWaveformLeft;
		
		private bool isOnFrameLoopRange;
		private bool isOnFrameLoopPoint;
		private Vector2 positionMousePrevious;
		private Texture2D textureCursorMove;
		private Texture2D textureCursorHorizontal;

		private PlayMusicInformation playMusicInformation;

		public ComponentLoopEditor( ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			objectLock = new object();

			mouseButtonPrevious = false;
			isOnFrameLoopRange = false;
			isOnFrameLoopPoint = false;
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
			textureCursorMove = ( Texture2D )Resources.Load( "Cursor/Move", typeof( Texture2D ) );
			textureCursorHorizontal = ( Texture2D )Resources.Load( "Cursor/Horizontal", typeof( Texture2D ) );
		}
		
		public void SetPlayer( string aFilePath, PlayMusicInformation aMusicInformation )
		{
			bool lIsMute = player.IsMute;
			bool lIsLoop = player.IsLoop;
			float lVolume = player.Volume;

			title = Path.GetFileNameWithoutExtension( aFilePath );
			player = ConstructorCollection.ConstructPlayer( aFilePath );

			if( aMusicInformation.isSelected == true )
			{
				SetLoop( aMusicInformation.loopPoint );
			}

			player.IsMute = lIsMute;
			player.IsLoop = lIsLoop;
			player.Volume = lVolume;

			playMusicInformation = aMusicInformation;
			componentLoopSelector.SetPlayMusicInformation( aMusicInformation );
		}

		public void UpdateMesh()
		{
			MusicPcm lMusicPcm = ( MusicPcm )player.Music;
		
			if( lMusicPcm != null )
			{
				Vector3[] vertices = meshFilter.mesh.vertices;
				float[] lValueArray = new float[vertices.Length];

				RiffWaveRiff lRiffWaveRiff = new RiffWaveRiff( player.GetFilePath() );
				WaveformPcm lWaveform = new WaveformPcm( lRiffWaveRiff );
				waveform = new sbyte[lWaveform.format.samples];

				int lIndexPre = 0;

				for( int i = 0; i < lValueArray.Length; i++ )
				{
					lValueArray[i] = 0.0f;
				}

				for( int i = 0; i < lWaveform.format.samples; i++ )
				{
					waveform[i] = lWaveform.data.GetSampleByte( 0, i );
					
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
						
						vertices[lIndexPre * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lValueArray[lIndexPre * 2 + 0] / 4.0f ), 0.0f );
						vertices[lIndexPre * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lValueArray[lIndexPre * 2 + 1] / 4.0f ), 0.0f );
						
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
			if( GUI.Button( new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width ), 60.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width ), 60.0f ), new GUIContent( "", "StyleLoopTool.Frame" ), GuiStyleSet.StyleLoopTool.frame ) == true )
			{

			}
			
			if( GUI.Button( new Rect( ( float )( player.Loop.end.sample / player.GetLength().sample * Screen.width ) - 8.0f, 46.0f, 16.0f, 16.0f ), new GUIContent( "", "StyleLoopTool.ButtonPoint" ), GuiStyleSet.StyleLoopTool.buttonPoint ) == true )
			{
				
			}

			if( Input.GetMouseButton( 0 ) != mouseButtonPrevious )
			{
				if( Input.GetMouseButton( 0 ) == true )
				{
					Rect lFrameLoopRange = new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width ), 60.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width ), 60.0f );

					if( lFrameLoopRange.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoopRange = true;
						meshRenderer3.material.color = new Color( 0.0f, 0.5f, 0.5f, 0.5f );
						Cursor.SetCursor( textureCursorMove, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
					}
					
					Rect lFrameLoopPoint = new Rect( ( float )( player.Loop.end.sample / player.GetLength().sample * Screen.width ) - 8.0f, 46.0f, 16.0f, 16.0f );
					
					if( lFrameLoopPoint.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoopPoint = true;
						Cursor.SetCursor( textureCursorHorizontal, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
					}
				}
				else
				{
					isOnFrameLoopRange = false;
					isOnFrameLoopPoint = false;
					meshRenderer3.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
					Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto );
				}
			}

			if( isOnFrameLoopRange == true )
			{
				double lPositionStart = player.Loop.start.sample;

				lPositionStart += ( Event.current.mousePosition.x - positionMousePrevious.x ) * player.GetLength().sample / Screen.width;
				
				if( lPositionStart < 0.0d )
				{
					lPositionStart = 0.0d;
				}

				if( lPositionStart + player.Loop.length.sample > player.GetLength().sample + 2 )
				{
					lPositionStart = player.GetLength().sample - player.Loop.length.sample + 2;
				}

				if( lPositionStart != player.Loop.start.sample )
				{
					SetLoop( new LoopInformation( player.Loop.length.sampleRate, ( int )lPositionStart, ( int )lPositionStart + ( int )player.Loop.length.sample - 1 ) );
					playMusicInformation.loopPoint = player.Loop;
					playMusicInformation.music.Loop = player.Loop;
					playMusicInformation.isSelected = true;
				}
			}
			
			if( isOnFrameLoopPoint == true )
			{
				double lPositionEnd = player.Loop.end.sample;
				
				lPositionEnd += ( Event.current.mousePosition.x - positionMousePrevious.x ) * player.GetLength().sample / Screen.width;
				
				if( lPositionEnd < player.Loop.start.sample )
				{
					lPositionEnd = player.Loop.start.sample;
				}
				
				if( lPositionEnd >= player.GetLength().sample )
				{
					lPositionEnd = player.GetLength().sample - 1;
				}
				
				if( lPositionEnd != player.Loop.start.sample )
				{
					SetLoop( new LoopInformation( ( int )player.Loop.start.sampleRate, ( int )player.Loop.start.sample, ( int )lPositionEnd ) );
					playMusicInformation.loopPoint = player.Loop;
					playMusicInformation.music.Loop = player.Loop;
					playMusicInformation.isSelected = true;
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

		public LoopInformation GetLoop()
		{
			return player.Loop;
		}

		public void SetLoop( LoopInformation aLoopInformation )
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
					
					vertices[lIndexPre * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lValueArray[lIndexPre * 2 + 0] / 4.0f ), 0.0f );
					vertices[lIndexPre * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lValueArray[lIndexPre * 2 + 1] / 4.0f ), 0.0f );
					
					lIndexPre = lIndex;
				}
			}
			
			meshFilter2.mesh.vertices = vertices;
			meshFilter2.mesh.RecalculateBounds();
			
			objectWaveformRight.SetPosition( ( aLoopInformation.length.sample ) / player.GetLength().sample );
		}
	}
}
