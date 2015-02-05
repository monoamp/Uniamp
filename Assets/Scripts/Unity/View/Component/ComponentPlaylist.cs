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
	public class DataLoopPlaylist
	{
		public DirectoryInfo directoryInfo;

		public List<string> filePathList;
		public Dictionary<string, bool> isSelectedList;
		public Dictionary<string, List<LoopInformation>> loopPointListDictionary;
		
		public delegate void PlayMusic( string aName );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		public DataLoopPlaylist( DirectoryInfo aDirectoryInfo, PlayMusic aSetPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			directoryInfo = aDirectoryInfo;

			playMusic = aSetPlayMusic;
			getPlayingMusic = aGetPlayingMusic;
		}
	}

	public class ComponentPlaylist : IView
	{
		public DataLoopPlaylist data;
        private Dictionary<string, IMusic> musicDictionary;
		private Vector2 scrollPosition;
		private string[] pathArray;
		
		public Rect Rect{ get; set; }

		public ComponentPlaylist( DirectoryInfo aDirectoryInfo, DataLoopPlaylist.PlayMusic aSetFileInfoPlaying, DataLoopPlaylist.GetPlayingMusic aGetFileInfoPlaying )
		{
			data = new DataLoopPlaylist( aDirectoryInfo, aSetFileInfoPlaying, aGetFileInfoPlaying );
			data.loopPointListDictionary = new Dictionary<string, List<LoopInformation>>();
			musicDictionary = new Dictionary<string, IMusic>();

			UpdateFileList();

			scrollPosition = Vector2.zero;
		}

		public void SetDirectory( DirectoryInfo aDirectoryInfo )
		{
			data.directoryInfo = aDirectoryInfo;
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
			//float lWidthTable = Screen.width;
			float lWidthValue = 80.0f;
			float lWidthPartition = GuiStyleSet.StyleTable.partitionVertical.fixedWidth;
			//float lWidthVerticalbar = GuiStyleSet.StyleScrollbar.verticalbar.fixedWidth;
			//float lWidthName = lWidthTable - lWidthValue * 4 - lWidthPartition * 4 - lWidthVerticalbar;

			if( Event.current.type != EventType.Repaint )
			{
				UpdateFileList();
			}

			GUILayout.BeginVertical();
			{
				GUILayout.BeginScrollView( new Vector2( scrollPosition.x, 0.0f ), false, true, GuiStyleSet.StyleTable.horizontalbarHeader, GuiStyleSet.StyleTable.verticalbarHeader, GuiStyleSet.StyleGeneral.none );
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label( new GUIContent( "Name", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.MinWidth( 300.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Length", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( lWidthValue ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						
						GUILayout.BeginVertical( GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( lWidthValue * 3 + lWidthPartition * 2 ) );
						{
							GUILayout.Label( new GUIContent( "Loop", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeaderTop );
							GUILayout.Label( new GUIContent( "", "StyleGeneral.partitionHorizontal" ), GuiStyleSet.StyleGeneral.partitionHorizontal );

							GUILayout.BeginHorizontal();
							{
								GUILayout.Label( new GUIContent( "Start", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
								GUILayout.Label( new GUIContent( "End", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
								GUILayout.Label( new GUIContent( "Length", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
							}
							GUILayout.EndHorizontal();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				
				GUILayout.BeginHorizontal();
				{
					scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, true, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar, GuiStyleSet.StyleScrollbar.view );
					{
						GUIStyle[] lViewRow = { GuiStyleSet.StyleTable.viewRowA, GuiStyleSet.StyleTable.viewRowB };

						for( int i = 0; i < data.filePathList.Count; i++ )
						{
							string lFilePath = data.filePathList[i];

							if( musicDictionary.ContainsKey( lFilePath ) == true )
							{
								GUILayout.BeginHorizontal( lViewRow[i % 2] );
								{
									if( lFilePath == data.getPlayingMusic() )
									{
										if( GUILayout.Toggle( true, new GUIContent( Path.GetFileName( lFilePath ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == false )
										{
											data.playMusic( lFilePath );
										}
									}
									else
									{
										if( GUILayout.Toggle( false, new GUIContent( Path.GetFileName( lFilePath ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == true )
										{
											data.playMusic( lFilePath );
										}
									}

									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( musicDictionary[lFilePath].Length.MMSS, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( data.loopPointListDictionary[lFilePath][0].start.String, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( data.loopPointListDictionary[lFilePath][0].end.String, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( data.loopPointListDictionary[lFilePath][0].length.String, GuiStyleSet.StyleTable.textRow );
								}
								GUILayout.EndHorizontal();
							}
						}

						GUILayout.BeginHorizontal();
						{
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

		private void UpdateFileList()
		{
			string[] lPathArray = PoolFilePath.Get( data.directoryInfo );

			if( lPathArray != pathArray )
			{
				pathArray = lPathArray;

				LoadLoop();
			}
		}

		private void LoadLoop()
		{
			data.filePathList = new List<string>();

			for( int i = 0; i < pathArray.Length; i++ )
			{
				string lPath = pathArray[i];
				data.filePathList.Add( lPath );
				Logger.BreakDebug( "Input:" + lPath );
				
				if( musicDictionary.ContainsKey( lPath ) == false )
				{
					IMusic lMusic = null;

					try
					{
						lMusic = LoaderCollection.LoadMusic( lPath );
					}
					catch( Exception aExpection )
					{
						Logger.BreakError( "LoopPlaylist Exception:" + aExpection.ToString() + ":" + lPath );
					}

					if( lMusic != null )
					{
						Logger.BreakDebug( "Add:" + lPath );
						musicDictionary.Add( lPath, lMusic );
						List<LoopInformation> lLoopPointList = new List<LoopInformation>();
						lLoopPointList.Add( lMusic.GetLoop( 0, 0 ) );

						if( data.loopPointListDictionary.ContainsKey( lPath ) == false )
						{
							data.loopPointListDictionary.Add( lPath, lLoopPointList );
						}
					}
				}
			}
		}

		public void ChangeMusicPrevious()
		{
			int lIndex = data.filePathList.IndexOf( data.getPlayingMusic() );

			if( lIndex >= 0 )
			{
				lIndex--;

				if( lIndex < 0 )
				{
					lIndex = data.filePathList.Count - 1;
				}

				data.playMusic( data.filePathList[lIndex] );
			}
		}

		public void ChangeMusicNext()
		{
			int lIndex = data.filePathList.IndexOf( data.getPlayingMusic() );

			if( lIndex >= 0 )
			{
				lIndex++;

				if( lIndex >= data.filePathList.Count )
				{
					lIndex = 0;
				}
				
				data.playMusic( data.filePathList[lIndex] );
			}
		}
    }
}

