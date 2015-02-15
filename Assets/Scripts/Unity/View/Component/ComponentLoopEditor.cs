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

		private MeshFilter meshFilterDetail;
		private MeshFilter meshFilterDetailLeft;
		private MeshFilter meshFilterDetailRight;
		private MeshFilter meshFilterAbstract;
		private MeshFilter meshFilterAbstractLeft;
		private MeshFilter meshFilterAbstractRight;
		
		private MeshRenderer meshRendererDetail;
		private MeshRenderer meshRendererDetailLeft;
		private MeshRenderer meshRendererDetailRight;
		private MeshRenderer meshRendererAbstract;
		private MeshRenderer meshRendererAbstractLeft;
		private MeshRenderer meshRendererAbstractRight;

		private sbyte[] waveform;
		
		private object objectLock;

		private ObjectWaveform objectWaveformDetailLeft;
		private ObjectWaveform objectWaveformDetailRight;
		private ObjectWaveform objectWaveformAbstractLeft;
		private ObjectWaveform objectWaveformAbstractRight;

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
			
			Mesh lMeshDetail = new Mesh();
			Mesh lMeshDetailLeft = new Mesh();
			Mesh lMeshDetailRight = new Mesh();
			Mesh lMeshAbstract = new Mesh();
			Mesh lMeshAbstractLeft = new Mesh();
			Mesh lMeshAbstractRight = new Mesh();

			Vector3[] vertices = new Vector3[1281 * 2];
			int[] lIndices = new int[1281 * 2];

			for( int i = 0; i < lIndices.Length; i++ )
			{
				lIndices[i] = i;
			}
			
			lMeshDetail.vertices = vertices;
			lMeshDetail.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshDetail.RecalculateBounds();
			
			lMeshDetailLeft.vertices = vertices;
			lMeshDetailLeft.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshDetailLeft.RecalculateBounds();

			lMeshDetailRight.vertices = vertices;
			lMeshDetailRight.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshDetailRight.RecalculateBounds();

			lMeshAbstract.vertices = vertices;
			lMeshAbstract.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshAbstract.RecalculateBounds();
			
			lMeshAbstractLeft.vertices = vertices;
			lMeshAbstractLeft.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshAbstractLeft.RecalculateBounds();

			lMeshAbstractRight.vertices = vertices;
			lMeshAbstractRight.SetIndices( lIndices, MeshTopology.Lines, 0 );
			lMeshAbstractRight.RecalculateBounds();

			GameObject gameObjectWaveformDetail = GameObject.Find( "WaveformDetail" );
			GameObject gameObjectWaveformDetailLeft = GameObject.Find( "WaveformDetailLeft" );
			GameObject gameObjectWaveformDetailRight = GameObject.Find( "WaveformDetailRight" );
			GameObject gameObjectWaveformAbstract = GameObject.Find( "WaveformAbstract" );
			GameObject gameObjectWaveformAbstractLeft = GameObject.Find( "WaveformAbstractLeft" );
			GameObject gameObjectWaveformAbstractRight = GameObject.Find( "WaveformAbstractRight" );

			meshFilterDetail = gameObjectWaveformDetail.GetComponent<MeshFilter>();
			meshFilterDetailLeft = gameObjectWaveformDetailLeft.GetComponent<MeshFilter>();
			meshFilterDetailRight = gameObjectWaveformDetailRight.GetComponent<MeshFilter>();
			meshFilterAbstract= gameObjectWaveformAbstract.GetComponent<MeshFilter>();
			meshFilterAbstractLeft = gameObjectWaveformAbstractLeft.GetComponent<MeshFilter>();
			meshFilterAbstractRight = gameObjectWaveformAbstractRight.GetComponent<MeshFilter>();

			meshFilterDetail.sharedMesh = lMeshDetail;
			meshFilterDetailLeft.sharedMesh = lMeshDetailLeft;
			meshFilterDetailRight.sharedMesh = lMeshDetailRight;
			meshFilterAbstract.sharedMesh = lMeshAbstract;
			meshFilterAbstractLeft.sharedMesh = lMeshAbstractLeft;
			meshFilterAbstractRight.sharedMesh = lMeshAbstractRight;
			
			meshFilterDetail.sharedMesh.name = "WaveformDetail";
			meshFilterDetailLeft.sharedMesh.name = "WaveformDetailLeft";
			meshFilterDetailRight.sharedMesh.name = "WaveformDetailRight";
			meshFilterAbstract.sharedMesh.name = "WaveformAbstract";
			meshFilterAbstractLeft.sharedMesh.name = "WaveformAbstractLeft";
			meshFilterAbstractRight.sharedMesh.name = "WaveformAbstractRight";
			
			meshRendererDetail = gameObjectWaveformDetail.GetComponent<MeshRenderer>();
			meshRendererDetailLeft = gameObjectWaveformDetailLeft.GetComponent<MeshRenderer>();
			meshRendererDetailRight = gameObjectWaveformDetailRight.GetComponent<MeshRenderer>();
			meshRendererAbstract = gameObjectWaveformAbstract.GetComponent<MeshRenderer>();
			meshRendererAbstractLeft = gameObjectWaveformAbstractLeft.GetComponent<MeshRenderer>();
			meshRendererAbstractRight = gameObjectWaveformAbstractRight.GetComponent<MeshRenderer>();

			meshRendererDetail.material.color = new Color( 0.0f, 0.0f, 1.0f, 0.5f );
			meshRendererDetailLeft.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
			meshRendererDetailRight.material.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
			meshRendererAbstract.material.color = new Color( 0.0f, 0.0f, 1.0f, 0.5f );
			meshRendererAbstractLeft.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
			meshRendererAbstractRight.material.color = new Color( 1.0f, 0.0f, 0.0f, 0.5f );
			
			objectWaveformDetailLeft = new ObjectWaveform( gameObjectWaveformDetailLeft.transform, meshFilterDetailLeft );
			objectWaveformDetailRight = new ObjectWaveform( gameObjectWaveformDetailRight.transform, meshFilterDetailRight );
			objectWaveformAbstractLeft = new ObjectWaveform( gameObjectWaveformAbstractLeft.transform, meshFilterAbstractLeft );
			objectWaveformAbstractRight = new ObjectWaveform( gameObjectWaveformAbstractRight.transform, meshFilterAbstractRight );

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
				Vector3[] vertices = meshFilterDetail.mesh.vertices;

				RiffWaveRiff lRiffWaveRiff = new RiffWaveRiff( player.GetFilePath() );
				WaveformPcm lWaveform = new WaveformPcm( lRiffWaveRiff );
				waveform = new sbyte[lWaveform.format.samples];
				
				waveform = lWaveform.data.sampleByteArray[0];

				ChangeScale();
				ChangeAbstract();
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
			float lHeightFrame = 60.0f;

			if( scaleRate == 0.0f )
			{
				lHeightFrame = 80.0f;
			}

			if( GUI.Button( new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ) - 8.0f, 46.0f, 16.0f, 16.0f ), new GUIContent( "", "StyleLoopTool.ButtonPoint" ), GuiStyleSet.StyleLoopTool.buttonPoint ) == true )
			{
				
			}

			if( GUI.Button( new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ), 60.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width * scale ), lHeightFrame ), new GUIContent( "", "StyleLoopTool.Frame" ), GuiStyleSet.StyleLoopTool.frame ) == true )
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

					Rect lFrameLoopRange = new Rect( ( float )( player.Loop.start.sample / player.GetLength().sample * Screen.width * scale - positionWaveform * Screen.width * scale ), 60.0f, ( float )( player.Loop.length.sample / player.GetLength().sample * Screen.width * scale ), lHeightFrame );

					if( lFrameLoopRange.Contains( Event.current.mousePosition ) == true )
					{
						isOnFrameLoopRange = true;
						meshRendererDetailLeft.material.color = new Color( 0.0f, 0.5f, 0.5f, 0.5f );
						Cursor.SetCursor( textureCursorMove, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
					}
				}
				else
				{
					isOnFrameLoopStart = false;
					isOnFrameLoopEnd = false;
					isOnFrameLoopRange = false;
					meshRendererDetailLeft.material.color = new Color( 0.0f, 1.0f, 0.0f, 0.5f );
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
				
				if( scaleRate != 0.0f )
				{
					float lPositionWaveformAfter = GUILayout.HorizontalScrollbar( positionWaveform, 1.0f / scale, 0.0f, 1.0f, GuiStyleSet.StyleLoopTool.waveformbar );

					if( lPositionWaveformAfter != positionWaveform )
					{
						positionWaveform = lPositionWaveformAfter;
						ChangeScale();
						UpdateVertex();
					}
				}
				else
				{
					GUILayout.HorizontalScrollbar( positionWaveform, 1.0f / scale, 0.0f, 1.0f, GuiStyleSet.StyleLoopTool.nullbar );
				}

				float lPositionFloat = ( float )player.PositionRate;
				float lPositionAfter = GUILayout.HorizontalSlider( lPositionFloat, 0.0f, 1.0f, GuiStyleSet.StylePlayer.seekbar, GuiStyleSet.StylePlayer.seekbarThumb );
				
				if( lPositionAfter != lPositionFloat )
				{
					player.PositionRate = lPositionAfter;
				}

				GUILayout.BeginHorizontal();
				{
					componentLoopSelector.OnGUI();
					
					GUILayout.FlexibleSpace();

					player.IsMute = GUILayout.Toggle( player.IsMute, new GUIContent( "", "StylePlayer.ToggleMute" ), GuiStyleSet.StylePlayer.toggleMute );
					
					if( player.IsMute == false )
					{
						player.Volume = GUILayout.HorizontalSlider( player.Volume, 0.0f, 1.0f, GuiStyleSet.StylePlayer.volumebar, GuiStyleSet.StyleSlider.horizontalbarThumb );
						
						if( player.Volume == 0.0f )
						{
							player.IsMute = true;
						}
					}
					else // isMute == true
					{
						float lVolume = GUILayout.HorizontalSlider( 0.0f, 0.0f, 1.0f, GuiStyleSet.StylePlayer.volumebar, GuiStyleSet.StyleSlider.horizontalbarThumb );
						
						if( lVolume != 0.0f )
						{
							player.IsMute = false;
							player.Volume = lVolume;
						}
					}

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

					if( GUILayout.Button( new GUIContent ( "-", "StyleGeneral.ButtonCircle" ), GuiStyleSet.StyleGeneral.buttonCircle ) == true )
					{

					}
					
					GUILayout.FlexibleSpace();

					float lScaleRateAfter = GUILayout.HorizontalSlider( scaleRate, 0.0f, 1.0f, GuiStyleSet.StylePlayer.volumebarImage, GuiStyleSet.StyleSlider.horizontalbarThumb );

					if( lScaleRateAfter != scaleRate )
					{
						scaleRate = lScaleRateAfter;
						float lPositionPrevious = 0.5f;

						if( scale != 1.0f )
						{
							lPositionPrevious = positionWaveform + 1.0f / scale / 2.0f;
						}

						scale = ( float )( 1.0f + player.GetLength().sample * scaleRate * scaleRate * scaleRate * scaleRate / Screen.width );
						positionWaveform = lPositionPrevious - 1.0f / scale / 2.0f;
						ChangeScale();
						ChangeAbstract();
						UpdateVertex();
					}

					if( GUILayout.Button( new GUIContent ( "+", "StyleGeneral.ButtonCircle" ), GuiStyleSet.StyleGeneral.buttonCircle ) == true )
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
				ChangeAbstractLoop( aLoopInformation );
			}

			player.SetLoop( aLoopInformation );
			UpdateVertex();
		}

		private void UpdateVertex()
		{
			objectWaveformDetailLeft.SetLoop( player.Loop, ( int )player.GetLength().sample, scale, positionWaveform );
			objectWaveformDetailRight.SetLoop( player.Loop, ( int )player.GetLength().sample, scale, positionWaveform - player.Loop.length.sample / player.GetLength().sample );
			objectWaveformAbstractLeft.SetLoop( player.Loop, ( int )player.GetLength().sample, 1.0f, 0.0f );
			objectWaveformAbstractRight.SetLoop( player.Loop, ( int )player.GetLength().sample, 1.0f, -player.Loop.length.sample / player.GetLength().sample );
		}
		
		private void ChangeScale()
		{
			Vector3[] lVertices = meshFilterDetail.mesh.vertices;
			Vector3[] lVerticesRight = meshFilterDetailRight.mesh.vertices;

			int diff = ( int )player.Loop.length.sample;// % ( waveform.Length / Screen.width );

			for( int i = 0; i < Screen.width; i++ )
			{
				sbyte lMax = 0;
				sbyte lMin = 0;
				
				sbyte lMaxRight = 0;
				sbyte lMinRight = 0;

				for( int j = ( int )( waveform.Length / Screen.width * i / scale ); j < waveform.Length / Screen.width * ( i + 1 ) / scale; j += ( int )Math.Ceiling( 20.0d / scale ) )
				{
					int lIndex = ( int )( waveform.Length * positionWaveform + j );
					int lIndexRight = ( int )( waveform.Length * positionWaveform + j - diff );
					
					if( lIndex >= 0 && lIndex < waveform.Length )
					{
						sbyte lValue = waveform[lIndex];

						if( lValue > lMax )
						{
							lMax = lValue;
						}

						if( lValue < lMin )
						{
							lMin = lValue;
						}
					}

					if( lIndexRight >= 0 && lIndexRight < waveform.Length )
					{
						sbyte lValue = waveform[lIndexRight];
						
						if( lValue > lMaxRight )
						{
							lMaxRight = lValue;
						}
						
						if( lValue < lMinRight )
						{
							lMinRight = lValue;
						}
					}
				}
				
				if( scaleRate == 0.0f )
				{
					double lX = -Screen.width / 2.0d + i;
					double lY = Screen.height / 2.0d - 1.0d - 100.0d;
					
					lVertices[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMax / 3.0f ), 0.0f );
					lVertices[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMin / 3.0f ), 0.0f );
					
					lVerticesRight[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMaxRight / 3.0f ), 0.0f );
					lVerticesRight[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMinRight / 3.0f ), 0.0f );
				}
				else
				{
					double lX = -Screen.width / 2.0d + i;
					double lY = Screen.height / 2.0d - 1.0d - 90.0d;
					
					lVertices[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMax / 4.0f ), 0.0f );
					lVertices[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMin / 4.0f ), 0.0f );
					
					lVerticesRight[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMaxRight / 4.0f ), 0.0f );
					lVerticesRight[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMinRight / 4.0f ), 0.0f );
				}
			}
			
			meshFilterDetail.mesh.vertices = lVertices;
			meshFilterDetail.mesh.RecalculateBounds();
			
			meshFilterDetailLeft.mesh.vertices = lVertices;
			meshFilterDetailLeft.mesh.RecalculateBounds();

			meshFilterDetailRight.mesh.vertices = lVerticesRight;
			meshFilterDetailRight.mesh.RecalculateBounds();
		}

		private void ChangeLoop( LoopInformation aLoopInformation )
		{
			Vector3[] lVerticesRight = meshFilterDetailRight.mesh.vertices;

			int diff = ( int )aLoopInformation.length.sample;// % ( waveform.Length / Screen.width );
			
			for( int i = 0; i < Screen.width; i++ )
			{
				sbyte lMaxRight = 0;
				sbyte lMinRight = 0;
				
				for( int j = ( int )( waveform.Length / Screen.width * i / scale ); j < waveform.Length / Screen.width * ( i + 1 ) / scale; j += ( int )Math.Ceiling( 20.0d / scale ) )
				{
					int lIndexRight = ( int )( waveform.Length * positionWaveform + j - diff );

					if( lIndexRight >= 0 && lIndexRight < waveform.Length )
					{
						sbyte lValue = waveform[lIndexRight];
						
						if( lValue > lMaxRight )
						{
							lMaxRight = lValue;
						}
						
						if( lValue < lMinRight )
						{
							lMinRight = lValue;
						}
					}
				}
				
				if( scaleRate == 0.0f )
				{
					double lX = -Screen.width / 2.0d + i;
					double lY = Screen.height / 2.0d - 1.0d - 100.0d;
					
					lVerticesRight[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMaxRight / 3.0f ), 0.0f );
					lVerticesRight[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMinRight / 3.0f ), 0.0f );
				}
				else
				{
					double lX = -Screen.width / 2.0d + i;
					double lY = Screen.height / 2.0d - 1.0d - 90.0d;

					lVerticesRight[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMaxRight / 4.0f ), 0.0f );
					lVerticesRight[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMinRight / 4.0f ), 0.0f );
				}
			}

			meshFilterDetailRight.mesh.vertices = lVerticesRight;
			meshFilterDetailRight.mesh.RecalculateBounds();
		}
		
		private void ChangeAbstract()
		{
			Vector3[] lVertices = meshFilterAbstract.mesh.vertices;
			Vector3[] lVerticesRight = meshFilterAbstractRight.mesh.vertices;

			int diff = ( int )player.Loop.length.sample;
			
			for( int i = 0; i < Screen.width; i++ )
			{
				sbyte lMax = 0;
				sbyte lMin = 0;
				
				sbyte lMaxRight = 0;
				sbyte lMinRight = 0;
				
				for( int j = ( int )( waveform.Length / Screen.width * i ); j < waveform.Length / Screen.width * ( i + 1 ); j += 20 )
				{
					int lIndexRight = j - ( int )player.Loop.length.sample;

					if( j >= 0 && j < waveform.Length )
					{
						sbyte lValue = waveform[j];
						
						if( lValue > lMax )
						{
							lMax = lValue;
						}
						
						if( lValue < lMin )
						{
							lMin = lValue;
						}
					}
					
					if( lIndexRight >= 0 && lIndexRight < waveform.Length )
					{
						sbyte lValue = waveform[lIndexRight];
						
						if( lValue > lMaxRight )
						{
							lMaxRight = lValue;
						}
						
						if( lValue < lMinRight )
						{
							lMinRight = lValue;
						}
					}
				}

				double lX = -Screen.width / 2.0d + i;
				double lY = Screen.height / 2.0d - 1.0d - 130.0d;
				
				if( scaleRate == 0.0f )
				{
					lY = Screen.height;
				}

				lVertices[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMax / 16.0f ), 0.0f );
				lVertices[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMin / 16.0f ), 0.0f );
				
				lVerticesRight[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMaxRight / 16.0f ), 0.0f );
				lVerticesRight[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMinRight / 16.0f ), 0.0f );
			}
			
			meshFilterAbstract.mesh.vertices = lVertices;
			meshFilterAbstract.mesh.RecalculateBounds();
			
			meshFilterAbstractLeft.mesh.vertices = lVertices;
			meshFilterAbstractLeft.mesh.RecalculateBounds();
			
			meshFilterAbstractRight.mesh.vertices = lVerticesRight;
			meshFilterAbstractRight.mesh.RecalculateBounds();
		}
		
		private void ChangeAbstractLoop( LoopInformation aLoopInformation )
		{
			Vector3[] lVerticesRight = meshFilterAbstractRight.mesh.vertices;
			
			for( int i = 0; i < Screen.width; i++ )
			{
				sbyte lMaxRight = 0;
				sbyte lMinRight = 0;
				
				for( int j = ( int )( waveform.Length / Screen.width * i ); j < waveform.Length / Screen.width * ( i + 1 ); j += 20 )
				{
					int lIndexRight = j - ( int )aLoopInformation.length.sample;

					if( lIndexRight >= 0 && lIndexRight < waveform.Length )
					{
						sbyte lValue = waveform[lIndexRight];
						
						if( lValue > lMaxRight )
						{
							lMaxRight = lValue;
						}
						
						if( lValue < lMinRight )
						{
							lMinRight = lValue;
						}
					}
				}
				
				double lX = -Screen.width / 2.0d + i;
				double lY = Screen.height / 2.0d - 1.0d - 130.0d;
				
				if( scaleRate == 0.0f )
				{
					lY = Screen.height;
				}

				lVerticesRight[i * 2 + 0] = new Vector3( ( float )lX, ( float )( lY + ( float )lMaxRight / 16.0f ), 0.0f );
				lVerticesRight[i * 2 + 1] = new Vector3( ( float )lX, ( float )( lY + ( float )lMinRight / 16.0f ), 0.0f );
			}

			meshFilterAbstractRight.mesh.vertices = lVerticesRight;
			meshFilterAbstractRight.mesh.RecalculateBounds();
		}
	}
}
