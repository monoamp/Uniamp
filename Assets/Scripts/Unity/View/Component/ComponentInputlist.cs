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
	public class DataLoopInputlist
	{
		public string[] pathArray;
		public Dictionary<string, double> progressDictionary;
		public Dictionary<string, bool> isSelectedDictionary;
		public Dictionary<string, LoopInformation> loopSetDictionary;
		public Dictionary<string, IMusic> musicDictionary;

		public DataLoopInputlist()
		{
			isSelectedDictionary = new Dictionary<string, bool>();
			progressDictionary = new Dictionary<string, double>();
			loopSetDictionary = new Dictionary<string ,LoopInformation>();
			musicDictionary = new Dictionary<string, IMusic>();
		}
	}

	public class ComponentInputlist : IView
    {
		public DataLoopInputlist data;
		public List<string> filePathList;
		private Vector2 scrollPosition;
		private bool isSelectedAll;
		private DirectoryInfo directoryInfo;

		private const string STRING_PROGRESS = "Progress";
		
		public delegate void PlayMusic( string aFilePath );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		public Rect Rect{ get; set; }

		public ComponentInputlist( DirectoryInfo aDirectoryInfo, PlayMusic aPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			directoryInfo = aDirectoryInfo;
			playMusic = aPlayMusic;
			getPlayingMusic = aGetPlayingMusic;

			data = new DataLoopInputlist();

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

		public void OnRenderObject()
		{
			
		}

		public void OnGUI()
		{
			//float widthVerticalbar = GuiStyleSet.StyleScrollbar.verticalbar.fixedWidth;

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
						bool lIsSelectedAll = isSelectedAll;
						isSelectedAll = GUILayout.Toggle( isSelectedAll, new GUIContent( "", "StyleTable.ToggleCheckHeader" ), GuiStyleSet.StyleTable.toggleCheckHeader );

						if( isSelectedAll != lIsSelectedAll )
						{
							foreach( string l in filePathList )
							{
								if( data.isSelectedDictionary.ContainsKey( l ) == true )
								{
									data.isSelectedDictionary[l] = isSelectedAll;
								}
							}
						}
						
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Name", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.MinWidth( 300.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Length", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 80.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Progress", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 140.0f ) );
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				
				GUILayout.BeginHorizontal();
				{
					scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, true, GuiStyleSet.StyleScrollbar.horizontalbar, GuiStyleSet.StyleScrollbar.verticalbar, GuiStyleSet.StyleScrollbar.view );
					{
						GUIStyle[] lViewRow = { GuiStyleSet.StyleTable.viewRowA, GuiStyleSet.StyleTable.viewRowB };

						for( int i = 0; i < filePathList.Count; i++ )
						{
							string lFilePath = filePathList[i];
							
							if( data.musicDictionary.ContainsKey( lFilePath ) == true )
							{
								GUILayout.BeginHorizontal( lViewRow[i % 2] );
								{
									data.isSelectedDictionary[lFilePath] = GUILayout.Toggle( data.isSelectedDictionary[lFilePath], new GUIContent( "", "StyleGeneral.ToggleCheck" ), GuiStyleSet.StyleGeneral.toggleCheck );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

									if( lFilePath == getPlayingMusic() )
									{
										if( GUILayout.Toggle( true, new GUIContent( Path.GetFileName( lFilePath ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == false )
										{
											playMusic( lFilePath );
										}
									}
									else
									{
										if( GUILayout.Toggle( false, new GUIContent( Path.GetFileName( lFilePath ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == true )
										{
											playMusic( lFilePath );
										}
									}
									
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
									GUILayout.TextField( data.musicDictionary[filePathList[i]].Length.MMSS, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

									if( data.isSelectedDictionary[lFilePath] == true )
									{
										if( data.progressDictionary[lFilePath] > 0.0f )
										{
											GUILayout.HorizontalScrollbar( 0.0f, ( float )data.progressDictionary[lFilePath], 0.0f, 1.01f, "progressbar", GUILayout.Width( 132.0f ) );
										}
										else
										{
											GUILayout.Label( new GUIContent( "", "StyleProgressbar.Progressbar" ), GuiStyleSet.StyleProgressbar.progressbar, GUILayout.Width( 132.0f ) );
										}
									}
									else
									{
										GUILayout.Label( new GUIContent( "", "StyleGeneral.None" ), GuiStyleSet.StyleGeneral.none, GUILayout.Width( 140.0f ) );
									}
								}
								GUILayout.EndHorizontal();
							}
						}

						GUILayout.BeginHorizontal();
						{
							GUILayout.BeginVertical( GUILayout.Width( 24.0f ) );
							{
								GUILayout.FlexibleSpace( );
							}
							GUILayout.EndVertical();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

							GUILayout.FlexibleSpace();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
							
							GUILayout.BeginVertical( GUILayout.Width( 140.0f ) );
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

			if( lPathArray != data.pathArray )
			{
				data.pathArray = lPathArray;
                
				filePathList = new List<string>();

				for( int i = 0; i < data.pathArray.Length; i++ )
				{
					string lFilePath = data.pathArray[i];
					Logger.BreakDebug( "Input:" + lFilePath );
					filePathList.Add( lFilePath );

					if( data.musicDictionary.ContainsKey( lFilePath ) == false )
					{
						IMusic lMusic = null;
						
						try
						{
							lMusic = LoaderCollection.LoadMusic( lFilePath );
						}
						catch( Exception aExpection )
						{
							Logger.BreakError( "LoopInputlist Exception:" + aExpection.ToString() + ":" + lFilePath );
						}
						
						if( lMusic != null )
						{
							data.musicDictionary.Add( lFilePath, lMusic );
						}
					}
					
					if( data.musicDictionary.ContainsKey( lFilePath ) == true )
					{
						data.loopSetDictionary.Add( lFilePath, new LoopInformation( 44100, 0, 0 ) );
						data.isSelectedDictionary.Add( lFilePath, false );
						data.progressDictionary.Add( lFilePath, 0.0d );
					}
				}
			}
		}
		
		public void OnApplicationQuit()
		{
			foreach( string l in filePathList )
			{
				if( data.isSelectedDictionary.ContainsKey( l ) == true )
				{
					data.isSelectedDictionary[l] = false;
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
					lIndex = data.loopSetDictionary.Count - 1;
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

				if( lIndex >= data.loopSetDictionary.Count )
				{
					lIndex = 0;
				}
				
				playMusic( filePathList[lIndex] );
			}
		}
	}
}