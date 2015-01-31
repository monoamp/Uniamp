using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ApplicationPlayer : IView
	{
		private ComponentPlayer componentPlayer;
		private ComponentPlaylist componentPlaylist;
		private ComponentChangeDirectory componentChangeDirectory;
		
		public Rect Rect{ get; set; }

		private List<DirectoryInfo> directoryInfoRecentList;

		public ApplicationPlayer( DirectoryInfo aDirectoryInfo )
		{
			DirectoryInfo lDirectoryInfoInput = aDirectoryInfo;
			directoryInfoRecentList = new List<DirectoryInfo>();

			try
			{
				using( StreamReader u = new StreamReader( Application.streamingAssetsPath + "/Config/Player.ini" ) )
				{
					int count = 0;

					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( Directory.Exists( line ) == true )
						{
							if( count == 0 )
							{
								lDirectoryInfoInput = new DirectoryInfo( line );
							}

							if( count < 5 )
							{
								directoryInfoRecentList.Add( new DirectoryInfo( line ) );
							}

							count++;
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );

				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/Player.ini" ) )
				{
					Logger.BreakDebug( "Create Player.ini" );
				}
			}

			componentPlayer = new ComponentPlayer( null, ChangeMusicPrevious, ChangeMusicNext );
			componentPlaylist = new ComponentPlaylist( lDirectoryInfoInput, SetFileInfoPlaying, GetFileInfoPlaying );
			
			DirectoryInfo lDirectoryInfoRoot = new DirectoryInfo( Application.streamingAssetsPath );
			componentChangeDirectory = new ComponentChangeDirectory( lDirectoryInfoRoot, lDirectoryInfoInput, SetDirectoryInfo, directoryInfoRecentList );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}
		
		private void SetInput( DirectoryInfo aDirectoryInfo )
		{
			directoryInfoRecentList.Insert( 0, aDirectoryInfo );
			
			try
			{
				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/Player.ini", false ) )
				{
					foreach( DirectoryInfo l in directoryInfoRecentList )
					{
						u.WriteLine( l.FullName );
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
				Logger.BreakDebug( "Failed write Player.ini" );
			}

			componentPlaylist = new ComponentPlaylist( aDirectoryInfo, PlayMusic, GetPlayingMusic );
		}
		
		private void PlayMusic( string aName )
		{
			componentPlayer.SetPlayer( aName );
		}
		
		private string GetPlayingMusic()
		{
			return componentPlayer.GetFilePath();
		}

		private void SetFileInfoPlaying( string aName )
		{
			componentPlayer.SetPlayer( aName );
		}
		
		private string GetFileInfoPlaying()
		{
			return componentPlayer.GetFilePath();
		}
		
		private void SetDirectoryInfo( DirectoryInfo aDirectoryInfo )
		{
			Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto );
			SetInput( aDirectoryInfo );
			componentPlaylist = new ComponentPlaylist( aDirectoryInfo, SetFileInfoPlaying, GetFileInfoPlaying );
		}

		public void ChangeMusicPrevious()
		{
			componentPlaylist.ChangeMusicPrevious();
		}
		
		public void ChangeMusicNext()
		{
			componentPlaylist.ChangeMusicNext();
		}
		
		public void Select()
		{
			componentPlayer.Select();
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
				componentPlayer.OnGUI();
				componentPlaylist.OnGUI();
				componentChangeDirectory.OnGUI();
			}
			GUILayout.EndVertical();
		}

		public void OnRenderObject()
		{
			componentPlayer.Rect = new Rect( 0.0f, 0.0f, Screen.width, Screen.height );
			componentPlayer.OnRenderObject();
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			componentPlayer.OnAudioFilterRead( aSoundBuffer, aChannels, aSampleRate );
		}

		public void OnApplicationQuit()
		{
			
		}
	}
}
