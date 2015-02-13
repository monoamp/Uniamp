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
		private MeshFilter meshFilterLeft;
		private MeshFilter meshFilterRight;
		
		private MeshRenderer meshRenderer;
		private MeshRenderer meshRendererLeft;
		private MeshRenderer meshRendererRight;

		private sbyte[] waveform;
		
		private object objectLock;

		private ObjectWaveform objectWaveformLeft;
		private ObjectWaveform objectWaveformRight;

		private bool isOnFrameLoopStart;
		private bool isOnFrameLoopEnd;
		private bool isOnFrameLoopRange;
		private Vector2 positionMousePrevious;
		private Texture2D textureCursorMove;
		private Texture2D textureCursorHorizontal;
		
		private float scale;
		private float scaleRate;
		private float positionWaveform;

		private PlayMusicInformation playMusicInformation;

		public ComponentLoopEditor( ChangeMusicPrevious aChangeMusicPrevious, ChangeMusicNext aChangeMusicNext )
		{
			objectLock = new object();

			mouseButtonPrevious = false;
			isOnFrameLoopStart = false;
			isOnFrameLoopEnd = false;
			isOnFrameLoopRange = false;
			positionMousePrevious = Vector2.zero;

			title = "";
			player = new PlayerNull();

			changeMusicPrevious = aChangeMusicPrevious;
			changeMusicNext = aChangeMusicNext;

			positionInBuffer = 0;
			
			Mesh lMesh = new Mesh();
			Mesh lMeshRight = new Mesh();
			Mesh lMeshLeft = new Mesh();

			Vector3[] vertices = new Vector3[1281 * 2];
			int[] lIndices = new int[1281 * 2];

			for( int i = 0; i < lIndices.Length; i++ )
			{
				lIndices[i] = i;
			}
			
			lMesh.vertices = vertices;
			lMesh.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMesh.RecalculateBounds();
			
			lMeshRight.vertices = vertices;
			lMeshRight.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshRight.RecalculateBounds();
			
			lMeshLeft.vertices = vertices;
			lMeshLeft.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshLeft.RecalculateBounds();

			GameObject gameObjectWaveform1 = GameObject.Find( "Waveform1" );
			GameObject gameObjectWaveformLeft = GameObject.Find( "Waveform3" );
			GameObject gameObjectWaveformRight = GameObject.Find( "Waveform2" );

			meshFilter = gameObjectWaveform1.GetComponent<MeshFilter>();
			meshFilterLeft = gameObjectWaveformLeft.GetComponent<MeshFilter>();
			meshFilterRight = gameObjectWaveformRight.GetComponent<MeshFilter>();

			meshFilter.sharedMesh = lMesh;
			meshFilterLeft.sharedMesh = lMeshLeft;
			meshFilterRight.sharedMesh = lMeshRight;
			
			meshFilter.sharedMesh.name = "Waveform1";
			meshFilterLeft.sharedMesh.name = "Waveform3";
			meshFilterRight.sharedMesh.name = "Waveform2";
			
			meshRenderer = gameObjectWaveform1.GetComponent<MeshRenderer>();
			meshRendererLeft = gameObjectWaveformLeft.GetComponent<MeshRenderer>();
			meshRendererRight = gameObjectWaveformRight.GetComponent<MeshRenderer>();

			meshRenderer.material.color = new Color( 0.0f, 0.0f, 1.0f, 0.5f );
			meshRendererLeft.material.color = new Color( 0.0f, 0.1f, 0.0f, 0.5f );
			meshRendererRight.material.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
			
			objectWaveformLeft = new ObjectWaveform( gameObjectWaveformLeft.transform, meshFilterLeft );
			objectWaveformRight = new ObjectWaveform( gameObjectWaveformRight.transform, meshFilterRight );

			componentLoopSelector = new ComponentLoopSelector( this );
			textureCursorMove = ( Texture2D )Resources.Load( "Cursor/Move", typeof( Texture2D ) );
			textureCursorHorizontal = ( Texture2D )Resources.Load( "Cursor/Horizontal", typeof( Texture2D ) );

			scale = 1.0f;
			scaleRate = 0.0f;
			positionWaveform = 0.0f;
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

				RiffWaveRiff lRiffWaveRiff = new RiffWaveRiff( player.GetFilePath() );
				WaveformPcm lWaveform = new WaveformPcm( lRiffWaveRiff );
				waveform = new sbyte[lWaveform.format.samples];
				
				waveform = lWaveform.data.sampleByteArray[0];
				/*
				for( int i = 0; i < lWaveform.format.samples; i++ )
				{
					waveform[i] = lWaveform.data.GetSampleByte( 0, i );
				}*/
				
				for( int i = 0; i < Screen.width; i++ )
				{
					sbyte lMax = 1;
					sbyte lMin = -1;

					for( float j = 0; j < waveform.Length / Screen.width / scale; j += Screen.width / 100 / scale )
					{
						float lIndex = waveform.Length / Screen.width * i / scale + j;
						
						sbyte lValue = waveform[( int )lIndex];
						
						if( lValue > lMax )
						{
							lMax = lValue;
						}

						if( lValue < lMin )
						{
							lMin = lValue;
						}
					}
					
					double lX = -Screen.width / 2.0d + i;
					double lY = Screen.height / 2.0d - 1.0d - 90.0d;
					
					vertices[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMax / 4.0f ), 0.0f );
					vertices[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMin / 4.0f ), 0.0f );
				}

				meshFilter.mesh.vertices = vertices;
				meshFilter.mesh.RecalculateBounds();

				meshFilterLeft.mesh.vertices = vertices;
				meshFilterLeft.mesh.RecalculateBounds();
				
				meshFilterRight.mesh.vertices = vertices;
				meshFilterRight.mesh.RecalculateBounds();

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
			if( GUI.Button( new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ) - 8.0f, 46.0f, 16.0f, 16.0f ), new GUIContent( "", "StyleLoopTool.ButtonPoint" ), GuiStyleSet.StyleLoopTool.buttonPoint ) == true )
			{
				
			}

			if( GUI.Button( new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ), 60.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width * scale ), 60.0f ), new GUIContent( "", "StyleLoopTool.Frame" ), GuiStyleSet.StyleLoopTool.frame ) == true )
			{

			}
			
			if( GUI.Button( new Rect( ( float )( player.Loop.end.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ) - 8.0f, 46.0f, 16.0f, 16.0f ), new GUIContent( "", "StyleLoopTool.ButtonPoint" ), GuiStyleSet.StyleLoopTool.buttonPoint ) == true )
			{
				
			}

			if( Input.GetMouseButton( 0 ) != mouseButtonPrevious )
			{
				if( Input.GetMouseButton( 0 ) == true )
				{
					Rect lFrameLoopStart = new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ) - 8.0f, 46.0f, 16.0f, 16.0f );
					
					if( lFrameLoopStart.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoopStart = true;
						Cursor.SetCursor( textureCursorHorizontal, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
					}

					Rect lFrameLoopEnd = new Rect( ( float )( player.Loop.end.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ) - 8.0f, 46.0f, 16.0f, 16.0f );
					
					if( lFrameLoopEnd.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoopEnd = true;
						Cursor.SetCursor( textureCursorHorizontal, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
					}

					Rect lFrameLoopRange = new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ), 60.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width * scale ), 60.0f );

					if( lFrameLoopRange.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoopRange = true;
						meshRendererLeft.material.color = new Color( 0.0f, 0.5f, 0.5f, 0.5f );
						Cursor.SetCursor( textureCursorMove, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
					}
				}
				else
				{
					isOnFrameLoopStart = false;
					isOnFrameLoopEnd = false;
					isOnFrameLoopRange = false;
					meshRendererLeft.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
					Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto );
				}
			}
			
			if( isOnFrameLoopStart == true )
			{
				double lPositionStart = player.Loop.start.sample;
				
				lPositionStart += ( Event.current.mousePosition.x - positionMousePrevious.x ) * player.GetLength().sample / Screen.width / scale;
				
				if( lPositionStart < 0 )
				{
					lPositionStart = 0;
				}
				
				if( lPositionStart > player.Loop.end.sample )
				{
					lPositionStart = player.Loop.end.sample;
				}
				
				if( lPositionStart != player.Loop.start.sample )
				{
					SetLoop( new LoopInformation( ( int )player.Loop.end.sampleRate, ( int )lPositionStart, ( int )player.Loop.end.sample ) );
					playMusicInformation.loopPoint = player.Loop;
					playMusicInformation.music.Loop = player.Loop;
					playMusicInformation.isSelected = true;
				}
			}
			
			if( isOnFrameLoopEnd == true )
			{
				double lPositionEnd = player.Loop.end.sample;
				
				lPositionEnd += ( Event.current.mousePosition.x - positionMousePrevious.x ) * player.GetLength().sample / Screen.width / scale;
				
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

			if( isOnFrameLoopRange == true )
			{
				double lPositionStart = player.Loop.start.sample;

				lPositionStart += ( Event.current.mousePosition.x - positionMousePrevious.x ) * player.GetLength().sample / Screen.width / scale;
				
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

			mouseButtonPrevious = Input.GetMouseButton( 0 );
			positionMousePrevious = Event.current.mousePosition;
			
			GuiStyleSet.StylePlayer.seekbar.fixedWidth = Screen.width;
			GuiStyleSet.StylePlayer.seekbarImage.fixedWidth = Screen.width;
			
			GUILayout.BeginVertical( GuiStyleSet.StylePlayer.box );
			{
				GUILayout.TextArea( title, GuiStyleSet.StylePlayer.labelTitle );
				GUILayout.Label( new GUIContent( "", "StyleGeneral.None" ), GuiStyleSet.StyleGeneral.none, GUILayout.Height( 60.0f ) );
				
				float lPositionWaveformAfter = GUILayout.HorizontalScrollbar( positionWaveform, 1.0f / scale, 0.0f, 1.0f );

				if( lPositionWaveformAfter != positionWaveform )
				{
					positionWaveform = lPositionWaveformAfter;
					ChangeScale();
					ChangeLoop( player.Loop );
					UpdateVertex();
					objectWaveformRight.SetPosition( ( player.Loop.length.sample ) / player.GetLength().sample, scale, positionWaveform );
				}

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
					
					if( GUILayout.Button( new GUIContent ( "-", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
					{

					}
					
					float lScaleRateAfter = GUILayout.HorizontalSlider( scaleRate, 0.0f, 1.0f, GuiStyleSet.StylePlayer.volumebarImage, GuiStyleSet.StyleSlider.horizontalbarThumb );

					if( lScaleRateAfter != scaleRate )
					{
						scaleRate = lScaleRateAfter;
						scale = ( float )( 1.0f + player.GetLength().sample * scaleRate * scaleRate * scaleRate * scaleRate / Screen.width );
						ChangeScale();
						ChangeLoop( player.Loop );
						UpdateVertex();
						objectWaveformRight.SetPosition( ( player.Loop.length.sample ) / player.GetLength().sample, scale, positionWaveform );
					}

					if( GUILayout.Button( new GUIContent ( "+", "StyleGeneral.Button" ), GuiStyleSet.StyleGeneral.button ) == true )
					{
						
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		
		public void OnRenderObject()
		{
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( title ) ).y + GuiStyleSet.StylePlayer.labelTitle.margin.top + GuiStyleSet.StylePlayer.labelTitle.margin.bottom;
			float lY = Rect.y + lHeightTitle + 76.0f;

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
			objectWaveformRight.SetPosition( ( aLoopInformation.length.sample ) / player.GetLength().sample, scale, positionWaveform );
		}

		private void UpdateVertex()
		{
			objectWaveformLeft.SetLoop( player.Loop, ( int )player.GetLength().sample, scale, positionWaveform );
			objectWaveformRight.SetLoop( player.Loop, ( int )player.GetLength().sample, scale, 0 );
		}
		
		private void ChangeScale()
		{
			Vector3[] vertices = meshFilter.mesh.vertices;

			for( int i = 1; i < Screen.width; i++ )
			{
				sbyte lMax = 1;
				sbyte lMin = -1;

				for( float j = 0; j < waveform.Length / Screen.width / scale; j += Screen.width / 100 / scale )
				{
					float lIndex = waveform.Length * positionWaveform + waveform.Length / Screen.width * i / scale + j;
					
					sbyte lValue = waveform[( int )lIndex];

					if( lValue > lMax )
					{
						lMax = lValue;
					}

					if( lValue < lMin )
					{
						lMin = lValue;
					}
				}

				double lX = -Screen.width / 2.0d + ( double )i;
				double lY = Screen.height / 2.0d - 1.0d - 90.0d;
				
				vertices[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMax / 4.0f ), 0.0f );
				vertices[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMin / 4.0f ), 0.0f );
			}
			
			meshFilter.mesh.vertices = vertices;
			meshFilter.mesh.RecalculateBounds();
			
			meshFilterLeft.mesh.vertices = vertices;
			meshFilterLeft.mesh.RecalculateBounds();
		}

		private void ChangeLoop( LoopInformation aLoopInformation )
		{
			Vector3[] vertices = meshFilterRight.mesh.vertices;

			int len = waveform.Length / Screen.width;
			int diff = 0;//( int )aLoopInformation.length.sample % len;

			for( int i = 1; i < Screen.width; i++ )
			{
				sbyte lMax = 1;
				sbyte lMin = -1;

				for( float j = 0; j < waveform.Length / Screen.width / scale; j += Screen.width / 100 / scale )
				{
					float lIndex = waveform.Length / Screen.width * i / scale + j;
					
					sbyte lValue = waveform[( int )lIndex - diff];

					if( lValue > lMax )
					{
						lMax = lValue;
					}

					if( lValue < lMin )
					{
						lMin = lValue;
					}
				}

				double lX = -Screen.width / 2.0d + i;
				double lY = Screen.height / 2.0d - 1.0d - 90.0d;
				
				vertices[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMax / 4.0f ), 0.0f );
				vertices[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMin / 4.0f ), 0.0f );
			}
			
			meshFilterRight.mesh.vertices = vertices;
			meshFilterRight.mesh.RecalculateBounds();
		}
	}
}
