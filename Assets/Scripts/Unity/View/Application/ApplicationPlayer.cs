using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View.Player
{
	public class ApplicationPlayer : IView
	{
		private MenuBar menu;
		private ComponentPlayer componentPlayer;
		private ComponentPlaylist componentPlaylist;
		private ComponentDirectoryBar componentDirectoryBar;
		
		public Rect Rect{ get; set; }

		public readonly List<DirectoryInfo> directoryInfoRecentList;

		public ApplicationPlayer( DirectoryInfo aDirectoryInfo )
		{
			directoryInfoRecentList = new List<DirectoryInfo>();

			ReadListDirectoryInfoRecent();

			if( directoryInfoRecentList.Count == 0 )
			{
				directoryInfoRecentList.Add( aDirectoryInfo );
			}

			menu = new MenuBar( Application.streamingAssetsPath + "/Language/Player/Menu/MenuBar.language", this );
			componentPlayer = new ComponentPlayer( null, ChangeMusicPrevious, ChangeMusicNext );
			componentPlaylist = new ComponentPlaylist( directoryInfoRecentList[0], SetFileInfoPlaying, GetFileInfoPlaying );
			componentDirectoryBar = new ComponentDirectoryBar( SetDirectoryInfo, directoryInfoRecentList );

			Rect = new Rect( 0.0f, 0.0f, 0.0f, 0.0f );
		}

		private void ReadListDirectoryInfoRecent()
		{
			try
			{
				using( StreamReader u = new StreamReader( Application.streamingAssetsPath + "/Config/Player.ini" ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( Directory.Exists( line ) == true )
						{
							directoryInfoRecentList.Add( new DirectoryInfo( line ) );

							if( directoryInfoRecentList.Count >= 5 )
							{
								break;
							}
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
		}

		public void SetInput( DirectoryInfo aDirectoryInfo )
		{
			for( int i = directoryInfoRecentList.Count - 1; i >= 0; i-- )
			{
				if( directoryInfoRecentList[i].FullName == aDirectoryInfo.FullName )
				{
					directoryInfoRecentList.RemoveAt( i );
				}
			}

			directoryInfoRecentList.Insert( 0, aDirectoryInfo );

			if( directoryInfoRecentList.Count > 5 )
			{
				directoryInfoRecentList.RemoveAt( 5 );
			}

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
		
		public void Awake()
		{
			componentPlayer.Awake();
		}

		public void Start()
		{

		}

		public void Update()
		{

		}

		public void OnGUI()
		{
			menu.OnGUI();

			GUILayout.BeginVertical();
			{
				componentPlayer.OnGUI();
				componentPlaylist.OnGUI();
				componentDirectoryBar.OnGUI();
			}
			GUILayout.EndVertical();
		}

		public void OnRenderObject()
		{
			float lHeightMenubar = GuiStyleSet.StyleMenu.bar.fixedHeight;
			componentPlayer.Rect = new Rect( 0.0f, lHeightMenubar, Screen.width, Screen.height - lHeightMenubar );
			componentPlayer.OnRenderObject();
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			componentPlayer.OnAudioFilterRead( aSoundBuffer, aChannels, aSampleRate );
		}

		public void OnApplicationQuit()
		{
			
		}
		
		public bool GetIsLoop()
		{
			return componentPlayer.GetIsLoop();
		}
		
		public void SetIsLoop( bool aIsLoop )
		{
			componentPlayer.SetIsLoop( aIsLoop );
		}
	}
}
