using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class ApplicationLoopTool : IView
	{
		private ViewMenu viewMenu;
		private ViewPlayer viewLoopPlayer;
		private ViewInputlist viewLoopInputlist;
		private ViewChangeDirectory viewChangeDirectoryInput;
		private ViewLoopSearch viewLoopSearch;
		private ViewPlaylist viewLoopPlaylist;
		private ViewChangeDirectory viewChangeDirectoryOutput;
		
		public Rect Rect{ get; set; }

		public ApplicationLoopTool( DirectoryInfo aDirectoryInfoInput, DirectoryInfo aDirectoryInfoOutput )
		{
			Debug.LogWarning( "dataPath:" + Application.dataPath );
			Debug.LogWarning( "persistentDataPath:" + Application.persistentDataPath );
			Debug.LogWarning( "streamingAssetsPath:" + Application.streamingAssetsPath );
			Debug.LogWarning( "temporaryCachePath:" + Application.temporaryCachePath );
			
			DirectoryInfo lDirectoryInfoInput = aDirectoryInfoInput;
			DirectoryInfo lDirectoryInfoOutput = aDirectoryInfoOutput;
			
			try
			{
				using( StreamReader u = new StreamReader( Application.streamingAssetsPath + "/Config/" + "LoopTool.ini" ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( line.IndexOf( "//" ) != 0 && line.Split( '=' ).Length >= 2 )
						{
							string opcode = line.Split( '=' )[0];
							string value = line.Split( '=' )[1];

							switch( opcode )
							{
							case "Input":
								if( Directory.Exists( value ) == true )
								{
									lDirectoryInfoInput = new DirectoryInfo( value );
								}
								break;

							case "Output":
								if( Directory.Exists( value ) == true )
								{
									lDirectoryInfoOutput = new DirectoryInfo( value );
								}
								break;
							
							default:
								break;
							}
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/" + "LoopTool.ini" ) )
				{
					Debug.LogWarning( "Create LoopTool.ini" );
				}
			}
			
			SetInput( lDirectoryInfoInput );
			SetOutput( lDirectoryInfoOutput );

			viewMenu = new ViewMenu();
			viewLoopPlayer = new ViewPlayer( null, ChangeMusicPrevious, ChangeMusicNext );
			
			DirectoryInfo lDirectoryInfoRoot = new DirectoryInfo( Application.streamingAssetsPath );
			viewChangeDirectoryInput = new ViewChangeDirectory( lDirectoryInfoRoot.Root, lDirectoryInfoInput, SetInput );
			viewChangeDirectoryOutput = new ViewChangeDirectory( lDirectoryInfoRoot.Root, lDirectoryInfoOutput, SetOutput );
		}

		private void PlayMusic( string aFilePath )
		{
			viewLoopPlayer = new ViewPlayer( aFilePath, ChangeMusicPrevious, ChangeMusicNext );
			viewLoopPlayer.Awake();
		}
		
		private string GetPlayingMusic()
		{
			return viewLoopPlayer.GetFilePath();
		}
		
		private void SetInput( DirectoryInfo aDirectoryInfo )
		{
			using( MemoryStream uMemoryStream = new MemoryStream() )
			{
				using( StreamWriter uStreamWriterMemory = new StreamWriter( uMemoryStream ) )
				{
					using( StreamReader uStreamReader = new StreamReader( Application.streamingAssetsPath + "/Config/" + "LoopTool.ini" ) )
					{
						bool lIsWrote = false;

						for( string line = uStreamReader.ReadLine(); line != null; line = uStreamReader.ReadLine() )
						{
							if( line.Split( '=' ).Length >= 1 && line.Split( '=' )[0] == "Input" )
							{
								uStreamWriterMemory.WriteLine( "Input=" + aDirectoryInfo.FullName );
								lIsWrote = true;
							}
							else
							{
								uStreamWriterMemory.WriteLine( line );
							}
						}

						if( lIsWrote == false )
						{
							Debug.LogWarning( "Input Write" );
							uStreamWriterMemory.WriteLine( "Input=" + aDirectoryInfo.FullName );
						}
					}

					uStreamWriterMemory.Flush();

					using( FileStream uFileStream = new FileStream( Application.streamingAssetsPath + "/Config/" + "LoopTool.ini", FileMode.Open, FileAccess.Write ) )
					{
						byte[] buffer = new byte[512];
						int numBytes;
						
						uMemoryStream.Position = 0;
						Debug.LogWarning( uMemoryStream.Position.ToString() + "/" + uMemoryStream.Length.ToString() );

						while( ( numBytes = uMemoryStream.Read( buffer, 0, 512 ) ) > 0 )
						{
							Debug.LogWarning( "Input:" + numBytes );
							uFileStream.Write( buffer, 0, numBytes );
						}
					}
				}
			}
			
			viewLoopInputlist = new ViewInputlist( aDirectoryInfo, PlayMusic, GetPlayingMusic );

			if( viewLoopPlaylist != null )
			{
				viewLoopSearch = new ViewLoopSearch( viewLoopPlaylist.data, viewLoopInputlist.data );
			}
		}
		
		private void SetOutput( DirectoryInfo aDirectoryInfo )
		{
			using( MemoryStream uMemoryStream = new MemoryStream() )
			{
				using( StreamWriter uStreamWriterMemory = new StreamWriter( uMemoryStream ) )
				{
					using( StreamReader uStreamReader = new StreamReader( Application.streamingAssetsPath + "/Config/" + "LoopTool.ini" ) )
					{
						bool lIsWrote = false;

						for( string line = uStreamReader.ReadLine(); line != null; line = uStreamReader.ReadLine() )
						{
							if( line.Split( '=' ).Length >= 1 && line.Split( '=' )[0] == "Output" )
							{
								uStreamWriterMemory.WriteLine( "Output=" + aDirectoryInfo.FullName );
								lIsWrote = true;
							}
							else
							{
								Debug.LogWarning( line );
								uStreamWriterMemory.WriteLine( line );
							}
						}

						if( lIsWrote == false )
						{
							Debug.LogWarning( "Output Write" );
							uStreamWriterMemory.WriteLine( "Output=" + aDirectoryInfo.FullName );
						}
					}
					
					uStreamWriterMemory.Flush();

					using( FileStream uFileStream = new FileStream( Application.streamingAssetsPath + "/Config/" + "LoopTool.ini", FileMode.Open, FileAccess.Write ) )
					{
						byte[] buffer = new byte[512];
						int numBytes;

						uMemoryStream.Position = 0;
						Debug.LogWarning( uMemoryStream.Position.ToString() + "/" + uMemoryStream.Length.ToString() );

						while( ( numBytes = uMemoryStream.Read( buffer, 0, 512 ) ) > 0 )
						{
							Debug.LogWarning( "Output:" + numBytes );
							uFileStream.Write( buffer, 0, numBytes );
						}
					}
				}
			}

			viewLoopPlaylist = new ViewPlaylist( aDirectoryInfo, PlayMusic, GetPlayingMusic );
			viewLoopSearch = new ViewLoopSearch( viewLoopPlaylist.data, viewLoopInputlist.data );
		}

		public void ChangeMusicPrevious()
		{
			viewLoopInputlist.ChangeMusicPrevious();
			viewLoopPlaylist.ChangeMusicPrevious();
		}
		
		public void ChangeMusicNext()
		{
			viewLoopInputlist.ChangeMusicNext();
			viewLoopPlaylist.ChangeMusicNext();
		}

		public void Awake()
		{
			viewLoopPlayer.Awake();
		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			
		}

		public void OnGUI()
        {
			viewMenu.OnGUI();
			viewLoopPlayer.OnGUI();
			
			float lHeightMenu = GuiStyleSet.StyleMenu.button.CalcSize( new GUIContent( "" ) ).y;
			float lHeightTitle = GuiStyleSet.StylePlayer.labelTitle.CalcSize( new GUIContent( "" ) ).y;
			float lY = lHeightMenu + lHeightTitle + GuiStyleSet.StyleGeneral.box.margin.top + GuiStyleSet.StyleGeneral.box.padding.top + GuiStyleSet.StylePlayer.seekbar.fixedHeight;

			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical( GUILayout.Width( Screen.width / 2.0f ) );
				{
					viewLoopSearch.OnGUI();
					GUILayout.Label( new GUIContent ( "Input", "StyleLoopTool.LabelInput" ), GuiStyleSet.StyleLoopTool.labelInput );
					GUILayout.Label( new GUIContent ( "", "StyleLoopTool.BackgroundInput" ), GuiStyleSet.StyleLoopTool.backgroundInput );
					viewChangeDirectoryInput.OnGUI();
					viewLoopInputlist.OnGUI();
				}
				GUILayout.EndVertical();

				GUILayout.BeginVertical( GUILayout.Width( Screen.width / 2.0f ) );
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Button( new GUIContent ( "Save", "StyleLoopTool.ButtonSave" ), GuiStyleSet.StyleLoopTool.buttonSave );
					}
					GUILayout.EndVertical();
					GUILayout.Label( new GUIContent ( "Output", "StyleLoopTool.LabelOutput" ), GuiStyleSet.StyleLoopTool.labelOutput );
					GUILayout.Label( new GUIContent ( "", "StyleLoopTool.BackgroundOutput" ), GuiStyleSet.StyleLoopTool.backgroundOutput );
					viewChangeDirectoryOutput.OnGUI();
					viewLoopPlaylist.OnGUI();
				}
				GUILayout.EndVertical();
			}
            GUILayout.EndHorizontal();
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			viewLoopPlayer.OnAudioFilterRead( aSoundBuffer, aChannels, aSampleRate );
		}
		
		public void OnApplicationQuit()
		{
			viewLoopSearch.OnApplicationQuit();
		}

		public void OnRenderObject()
		{
			viewLoopPlayer.OnRenderObject();
		}
	}
}
