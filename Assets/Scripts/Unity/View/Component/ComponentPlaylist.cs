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

namespace Unity.View
{
	public class DataLoopPlaylist
	{
		public DirectoryInfo directoryInfo;
		public DirectoryInfo directoryInfoRoot;

		public List<string> fileInfoList;
		public List<bool> isSelectedList;
		public List<List<LoopInformation>> loopPointListList;
		
		public delegate void PlayMusic( string aName );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		public DataLoopPlaylist( DirectoryInfo aDirectoryInfo, PlayMusic aSetPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			directoryInfo = aDirectoryInfo;
			directoryInfoRoot = new DirectoryInfo( Application.streamingAssetsPath );

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
			data.fileInfoList = new List<string>();
			data.loopPointListList = new List<List<LoopInformation>>();
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

						for( int i = 0; i < data.fileInfoList.Count; i++ )
						{
							GUILayout.BeginHorizontal( lViewRow[i % 2] );
							{
								if( data.fileInfoList[i] == data.getPlayingMusic() )
								{
									if( GUILayout.Toggle( true, new GUIContent( Path.GetFileName( data.fileInfoList[i] ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == false )
									{
										data.playMusic( data.fileInfoList[i] );
									}
								}
								else
								{
									if( GUILayout.Toggle( false, new GUIContent( Path.GetFileName( data.fileInfoList[i] ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == true )
									{
										data.playMusic( data.fileInfoList[i] );
									}
								}

								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
								GUILayout.TextField( musicDictionary[data.fileInfoList[i]].Length.MMSS, GuiStyleSet.StyleTable.textRow );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
								GUILayout.TextField( data.loopPointListList[i][0].start.String, GuiStyleSet.StyleTable.textRow );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
								GUILayout.TextField( data.loopPointListList[i][0].end.String, GuiStyleSet.StyleTable.textRow );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
								GUILayout.TextField( data.loopPointListList[i][0].length.String, GuiStyleSet.StyleTable.textRow );
							}
							GUILayout.EndHorizontal();
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
			for( int i = 0; i < pathArray.Length; i++ )
			{
				string lPath = pathArray[i];
				
				if( musicDictionary.ContainsKey( lPath ) == false )
				{
					IMusic lMusic = null;

					try
					{
						lMusic = LoaderCollection.LoadMusic( lPath );
					}
					catch( Exception aExpection )
					{
						Debug.LogWarning( "ViewLoopPlaylist Exception:" + aExpection.ToString() + ":" + lPath );
					}
				
					if( lMusic != null )
					{
						musicDictionary.Add( lPath, lMusic );
						data.fileInfoList.Add( lPath );
						List<LoopInformation> lLoopPointList = new List<LoopInformation>();
						lLoopPointList.Add( lMusic.GetLoop( 0, 0 ) );
						data.loopPointListList.Add( lLoopPointList );
					}
				}
			}
		}

		public void ChangeMusicPrevious()
		{
			int lIndex = data.fileInfoList.IndexOf( data.getPlayingMusic() );

			if( lIndex >= 0 )
			{
				lIndex--;

				if( lIndex < 0 )
				{
					lIndex = data.fileInfoList.Count - 1;
				}

				data.playMusic( data.fileInfoList[lIndex] );
			}
		}

		public void ChangeMusicNext()
		{
			int lIndex = data.fileInfoList.IndexOf( data.getPlayingMusic() );

			if( lIndex >= 0 )
			{
				lIndex++;

				if( lIndex >= data.fileInfoList.Count )
				{
					lIndex = 0;
				}
				
				data.playMusic( data.fileInfoList[lIndex] );
			}
		}
    }
}

