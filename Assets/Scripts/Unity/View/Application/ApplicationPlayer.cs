using UnityEngine;

using System;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ApplicationPlayer : IView
	{
		private ViewPlayer viewLoopPlayer;
		private ViewPlaylist viewLoopPlaylist;
		private ViewChangeDirectory viewChangeDirectory;
		
		public Rect Rect{ get; set; }

		public ApplicationPlayer( DirectoryInfo aDirectoryInfo )
		{
			DirectoryInfo lDirectoryInfoInput = aDirectoryInfo;

			try
			{
				using( StreamReader u = new StreamReader( Application.streamingAssetsPath + "/Config/Player.ini" ) )
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
									
								default:
									break;
							}
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.Debug( "Exception:" + aExpection );

				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/Player.ini" ) )
				{
					Debug.LogWarning( "Create Player.ini" );
				}
			}

			viewLoopPlayer = new ViewPlayer( null, ChangeMusicPrevious, ChangeMusicNext );
			viewLoopPlaylist = new ViewPlaylist( lDirectoryInfoInput, SetFileInfoPlaying, GetFileInfoPlaying );
			
			DirectoryInfo lDirectoryInfoRoot = new DirectoryInfo( Application.streamingAssetsPath );
			viewChangeDirectory = new ViewChangeDirectory( lDirectoryInfoRoot, lDirectoryInfoInput, SetDirectoryInfo );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
		
		private void SetInput( DirectoryInfo aDirectoryInfo )
		{
			using( MemoryStream uMemoryStream = new MemoryStream() )
			{
				using( StreamWriter uStreamWriter = new StreamWriter( uMemoryStream ) )
				{
					using( StreamReader uStreamReader = new StreamReader( Application.streamingAssetsPath + "/Config/Player.ini" ) )
					{
						bool lIsWrote = false;
						
						for( string line = uStreamReader.ReadLine(); line != null; line = uStreamReader.ReadLine() )
						{
							if( line.Split( '=' ).Length >= 1 && line.Split( '=' )[0] == "Input" )
							{
								uStreamWriter.WriteLine( "Input=" + aDirectoryInfo.FullName );
								lIsWrote = true;
							}
							else
							{
								uStreamWriter.WriteLine( line );
							}
						}
						
						if( lIsWrote == false )
						{
							uStreamWriter.WriteLine( "Input=" + aDirectoryInfo.FullName );
						}
					}
					
					uStreamWriter.Flush();

					using( FileStream uFileStream = new FileStream( Application.streamingAssetsPath + "/Config/Player.ini", FileMode.Create, FileAccess.Write ) )
					{
						byte[] lBuffer = new byte[uMemoryStream.Length];
						uMemoryStream.Position = 0;
						int lLength = uMemoryStream.Read( lBuffer, 0, ( int )uMemoryStream.Length );
						uFileStream.Write( lBuffer, 0, lLength );
					}
				}
			}
			
			viewLoopPlaylist = new ViewPlaylist( aDirectoryInfo, PlayMusic, GetPlayingMusic );
		}
		
		private void PlayMusic( string aName )
		{
			viewLoopPlayer.SetPlayer( aName );
		}
		
		private string GetPlayingMusic()
		{
			return viewLoopPlayer.GetFilePath();
		}

		private void SetFileInfoPlaying( string aName )
		{
			viewLoopPlayer.SetPlayer( aName );
		}
		
		private string GetFileInfoPlaying()
		{
			return viewLoopPlayer.GetFilePath();
		}
		
		private void SetDirectoryInfo( DirectoryInfo aDirectoryInfo )
		{
			Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto );
			SetInput( aDirectoryInfo );
			viewLoopPlaylist = new ViewPlaylist( aDirectoryInfo, SetFileInfoPlaying, GetFileInfoPlaying );
		}

		public void ChangeMusicPrevious()
		{
			viewLoopPlaylist.ChangeMusicPrevious();
		}
		
		public void ChangeMusicNext()
		{
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
			GUILayout.BeginVertical();
			{
				viewLoopPlayer.OnGUI();
				viewLoopPlaylist.OnGUI();
				viewChangeDirectory.OnGUI();
			}
			GUILayout.EndVertical();
		}
		
		public void OnApplicationQuit()
		{

		}

		public void OnRenderObject()
		{
			viewLoopPlayer.OnRenderObject();
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			viewLoopPlayer.OnAudioFilterRead( aSoundBuffer, aChannels, aSampleRate );
		}
	}
}
