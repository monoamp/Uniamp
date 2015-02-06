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

		public string[] pathArray;
		public List<string> filePathList;
		public DirectoryInfo directoryInfo;

		public Dictionary<string, PlayMusicInformation> data;
		private Vector2 scrollPosition;
		private bool isSelectedAll;

		public delegate void PlayMusic( string aName );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		public ComponentPlaylist( DirectoryInfo aDirectoryInfo, PlayMusic aPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			directoryInfo = aDirectoryInfo;
			playMusic = aPlayMusic;
			getPlayingMusic = aGetPlayingMusic;

			data = new Dictionary<string, PlayMusicInformation>();

			UpdateFileList();
			
			isSelectedAll = false;
			scrollPosition = Vector2.zero;
		}

		public void SetDirectory( DirectoryInfo aDirectoryInfo )
		{
			directoryInfo = aDirectoryInfo;
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
				UpdateFileList();
			}

			GUILayout.BeginVertical();
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
								GUILayout.Label( new GUIContent( "Start", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
								GUILayout.Label( new GUIContent( "End", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
								GUILayout.Label( new GUIContent( "Length", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.textHeader, GUILayout.Width( lWidthValue ) );
							}
							GUILayout.EndHorizontal();
						}
						GUILayout.EndVertical();
						
						if( isSelectedAll != lIsSelectedAllBefore )
						{
							foreach( string l in filePathList )
							{
								if( data.ContainsKey( l ) == true )
								{
									data[l].isSelected = isSelectedAll;
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

							if( data.ContainsKey( lFilePath ) == true )
							{
								GUILayout.BeginHorizontal( lViewRow[lCount % 2] );
								{
									lCount++;
									IMusic lMusic = data[lFilePath].music;

									data[lFilePath].isSelected = GUILayout.Toggle( data[lFilePath].isSelected, new GUIContent( "", "StyleGeneral.ToggleCheck" ), GuiStyleSet.StyleGeneral.toggleCheck );
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
									GUILayout.TextField( lMusic.Length.MMSS, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusic.Loop.start.String, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusic.Loop.end.String, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( lMusic.Loop.length.String, GuiStyleSet.StyleTable.textRow );
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

		private void UpdateFileList()
		{
			string[] lPathArray = PoolFilePath.Get( directoryInfo );
			
			if( lPathArray != pathArray )
			{
				pathArray = lPathArray;

				filePathList = new List<string>();

				for( int i = 0; i < pathArray.Length; i++ )
				{
					string lFilePath = pathArray[i];
					Logger.BreakDebug( "Input:" + lFilePath );
					
					if( data.ContainsKey( lFilePath ) == false )
					{
						IMusic lMusic = null;

						try
						{
							lMusic = LoaderCollection.LoadMusic( lFilePath );
						}
						catch( Exception aExpection )
						{
							Logger.BreakError( "LoopPlaylist Exception:" + aExpection.ToString() + ":" + lFilePath );
						}

						if( lMusic != null )
						{
							filePathList.Add( lFilePath );
							
							if( data.ContainsKey( lFilePath ) == false )
							{
								data.Add( lFilePath, new PlayMusicInformation( false, lMusic, lMusic.Loop ) );
							}

							/*
							List<LoopInformation> lLoopPointList = new List<LoopInformation>();

							for( int j = 0; j < data.musicDictionary[lFilePath].GetCountLoopX(); j++ )
							{
								for( int k = 0; k < data.musicDictionary[lFilePath].GetCountLoopY( j ); k++ )
								{
									lLoopPointList.Add( data.musicDictionary[lFilePath].GetLoop( j, k ) );
								}
							}*/
						}
					}
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

