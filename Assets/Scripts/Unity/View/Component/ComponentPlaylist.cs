using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Component.Sound;
using Monoamp.Common.Utility;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ComponentPlaylist : IView
	{
		public Rect Rect{ get; set; }

		public DirectoryInfo directoryInfo;
		public List<string> filePathList;
		public Dictionary<string, long> timeStampTicksDictionary;
		public Dictionary<string, PlayMusicInformation> musicInformationDictionary;

		private Vector2 scrollPosition;
		private bool isSelectedAll;

		public delegate void PlayMusic( string aName );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		private FileSystemWatcher fsw;

		public ComponentPlaylist( DirectoryInfo aDirectoryInfo, PlayMusic aPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			directoryInfo = aDirectoryInfo;
			playMusic = aPlayMusic;
			getPlayingMusic = aGetPlayingMusic;

			filePathList = new List<string>();
			timeStampTicksDictionary = new Dictionary<string, long>();
			musicInformationDictionary = new Dictionary<string, PlayMusicInformation>();

			UpdateFileList( null, null );
			
			isSelectedAll = false;
			scrollPosition = Vector2.zero;
			
			Logger.BreakDebug( "ComponentPlaylist:" + aDirectoryInfo.FullName );
			/*
			fsw = new FileSystemWatcher( aDirectoryInfo.FullName );
			fsw.Filter = "*.*";
			fsw.NotifyFilter = NotifyFilters.LastWrite;
			fsw.IncludeSubdirectories = false;
			fsw.Changed += new FileSystemEventHandler( UpdateFileList );
			fsw.EnableRaisingEvents = true;*/
		}

		public void SetDirectory( DirectoryInfo aDirectoryInfo )
		{
			directoryInfo = aDirectoryInfo;
			filePathList = new List<string>();
			timeStampTicksDictionary = new Dictionary<string, long>();
			musicInformationDictionary = new Dictionary<string, PlayMusicInformation>();
			
			isSelectedAll = false;
			scrollPosition = Vector2.zero;

			/*
			fsw.EnableRaisingEvents = false;
			fsw = new FileSystemWatcher( aDirectoryInfo.FullName );
			fsw.Filter = "*.*";
			fsw.NotifyFilter = NotifyFilters.LastWrite;
			fsw.IncludeSubdirectories = false;
			fsw.Changed += new FileSystemEventHandler( UpdateFileList );
			fsw.EnableRaisingEvents = true;*/
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
		
		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			
		}
		
		public void OnApplicationQuit()
		{
			
		}
		
		public void OnRenderObject()
		{
			
		}

		public void OnGUI()
		{
			float lWidthValue = 80.0f;
			float lWidthPartition = GuiStyleSet.StyleTable.partitionVertical.fixedWidth;

			if( Event.current.type != EventType.Repaint )
			{
				UpdateFileList( null, null );
			}

			GUILayout.BeginVertical( GuiStyleSet.StyleTable.box );
			{
				GUILayout.BeginScrollView( new Vector2( scrollPosition.x, 0.0f ), false, true, GuiStyleSet.StyleTable.horizontalbarHeader, GuiStyleSet.StyleTable.verticalbarHeader, GuiStyleSet.StyleGeneral.none );
				{
					GUILayout.BeginHorizontal();
					{
						bool lIsSelectedAllBefore = isSelectedAll;
						isSelectedAll = GUILayout.Toggle( isSelectedAll, new GUIContent( "", "StyleTable.ToggleCheckHeader" ), GuiStyleSet.StyleTable.toggleCheckHeader );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Name", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.MinWidth( 160.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Length", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( lWidthValue ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						
						GUILayout.BeginVertical( GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( lWidthValue * 3 + lWidthPartition * 2 ) );
						{
							GUILayout.Label( new GUIContent( "Loop", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeaderTop );
							GUILayout.Label( new GUIContent( "", "StyleGeneral.partitionHorizontal" ), GuiStyleSet.StyleGeneral.partitionHorizontal );

							GUILayout.BeginHorizontal();
							{
								GUILayout.Label( new GUIContent( "Start", "StyleTable.TextHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
								GUILayout.Label( new GUIContent( "End", "StyleTable.TextHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
								GUILayout.Label( new GUIContent( "Length", "StyleTable.TextHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
							}
							GUILayout.EndHorizontal();
						}
						GUILayout.EndVertical();
						
						if( isSelectedAll != lIsSelectedAllBefore )
						{
							foreach( string l in filePathList )
							{
								if( musicInformationDictionary.ContainsKey( l ) == true )
								{
									musicInformationDictionary[l].isSelected = isSelectedAll;
								}
							}
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				
				GUILayout.BeginHorizontal();
				{
					scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, true, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar, GuiStyleSet.StyleScrollbar.view );
					{
						GUIStyle[] lViewRow = { GuiStyleSet.StyleTable.viewRowA, GuiStyleSet.StyleTable.viewRowB };

						int lCount = 0;

						for( int i = 0; i < filePathList.Count; i++ )
						{
							string lFilePath = filePathList[i];

							if( musicInformationDictionary.ContainsKey( lFilePath ) == true )
							{
								PlayMusicInformation lMusicInformation = musicInformationDictionary[lFilePath];

								GUILayout.BeginHorizontal( lViewRow[lCount % 2] );
								{
									lCount++;

									lMusicInformation.isSelected = GUILayout.Toggle( lMusicInformation.isSelected, new GUIContent( "", "StyleGeneral.ToggleCheck" ), GuiStyleSet.StyleGeneral.toggleCheck );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

									if( lFilePath == getPlayingMusic() )
									{
										if( GUILayout.Toggle( true, new GUIContent( Path.GetFileName( lFilePath ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 160.0f ) ) == false )
										{
											playMusic( lFilePath );
										}
									}
									else
									{
										if( GUILayout.Toggle( false, new GUIContent( Path.GetFileName( lFilePath ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 160.0f ) ) == true )
										{
											playMusic( lFilePath );
										}
									}

									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusicInformation.music.Length.MMSS, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusicInformation.music.Loop.start.String, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusicInformation.music.Loop.end.String, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusicInformation.music.Loop.length.String, GuiStyleSet.StyleTable.textRow );
								}
								GUILayout.EndHorizontal();
							}
						}

						GUILayout.BeginHorizontal();
						{
							GUILayout.BeginVertical( GUILayout.Width( 24.0f ) );
							{
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

							GUILayout.FlexibleSpace();

							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
							
							GUILayout.BeginVertical( GUILayout.Width( lWidthValue ) );
							{
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
							
							GUILayout.BeginVertical( GUILayout.Width( lWidthValue ) );
							{
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

							GUILayout.BeginVertical( GUILayout.Width( lWidthValue ) );
							{
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
							
							GUILayout.BeginVertical( GUILayout.Width( lWidthValue ) );
							{
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		private void UpdateFileList( object sender, FileSystemEventArgs e )
		{
			string[] lFilePathArray = PoolFilePath.Get( directoryInfo );
			List<string> lFilePathNewList = new List<string>();

			// Check New File.
			for( int i = 0; i < lFilePathArray.Length; i++ )
			{
				string lFilePath = lFilePathArray[i];
				long lTimeStampTicks = File.GetLastWriteTime( lFilePath ).Ticks;

				if( timeStampTicksDictionary.ContainsKey( lFilePath ) == false )
				{
					timeStampTicksDictionary.Add( lFilePath, lTimeStampTicks );
					lFilePathNewList.Add( lFilePath );
				}
				else if( lTimeStampTicks != timeStampTicksDictionary[lFilePath] )
				{
					timeStampTicksDictionary[lFilePath] = lTimeStampTicks;
					lFilePathNewList.Add( lFilePath );
				}
			}

			for( int i = 0; i < lFilePathNewList.Count; i++ )
			{
				string lFilePath = lFilePathNewList[i];
				//Logger.BreakDebug( "Input:" + lFilePath );

				try
				{
					IMusic lMusic = ConstructorCollection.LoadMusic( lFilePath );

					if( lMusic != null )
					{
						if( musicInformationDictionary.ContainsKey( lFilePath ) == false )
						{
							filePathList.Add( lFilePath );
							musicInformationDictionary.Add( lFilePath, new PlayMusicInformation( timeStampTicksDictionary[lFilePath], false, lMusic, lMusic.Loop ) );
						}
						else
						{
							musicInformationDictionary[lFilePath] = new PlayMusicInformation( timeStampTicksDictionary[lFilePath], false, lMusic, lMusic.Loop );
						}
					}
				}
				catch( Exception aExpection )
				{
					Logger.BreakError( "LoopPlaylist Exception:" + aExpection.ToString() + ":" + lFilePath );
				}
			}
		}

		public void ChangeMusicPrevious()
		{
			int lIndex = filePathList.IndexOf( getPlayingMusic() );

			if( lIndex >= 0 )
			{
				lIndex--;

				if( lIndex < 0 )
				{
					lIndex = filePathList.Count - 1;
				}

				playMusic( filePathList[lIndex] );
			}
		}

		public void ChangeMusicNext()
		{
			int lIndex = filePathList.IndexOf( getPlayingMusic() );

			if( lIndex >= 0 )
			{
				lIndex++;

				if( lIndex >= filePathList.Count )
				{
					lIndex = 0;
				}
				
				playMusic( filePathList[lIndex] );
			}
		}
    }
}

