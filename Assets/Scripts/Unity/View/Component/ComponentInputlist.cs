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
	public class ComponentInputlist : IView
	{
		public Rect Rect{ get; set; }

		public Dictionary<string, InputMusicInformation> musicInformationDictionary;
		private long timeStampTicks;
		public List<string> filePathList;
		private Vector2 scrollPosition;
		private bool isSelectedAll;
		private DirectoryInfo directoryInfo;

		public delegate void PlayMusic( string aFilePath );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		public ComponentInputlist( DirectoryInfo aDirectoryInfo, PlayMusic aPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			timeStampTicks = 0;
			directoryInfo = aDirectoryInfo;
			playMusic = aPlayMusic;
			getPlayingMusic = aGetPlayingMusic;

			musicInformationDictionary = new Dictionary<string, InputMusicInformation>();

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
						GUILayout.Label( new GUIContent( "Length", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 80.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVerticalHeader );
						GUILayout.Label( new GUIContent( "Progress", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.Width( 140.0f ) );
						
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
								GUILayout.BeginHorizontal( lViewRow[lCount % 2] );
								{
									lCount++;

									musicInformationDictionary[lFilePath].isSelected = GUILayout.Toggle( musicInformationDictionary[lFilePath].isSelected, new GUIContent( "", "StyleGeneral.ToggleCheck" ), GuiStyleSet.StyleGeneral.toggleCheck );
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
									GUILayout.TextField( musicInformationDictionary[filePathList[i]].music.Length.MMSS, GuiStyleSet.StyleTable.textRow );
									GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

									if( musicInformationDictionary[lFilePath].isSelected == true )
									{
										if( musicInformationDictionary[lFilePath].progress > 0.0f )
										{
											GUILayout.HorizontalScrollbar( 0.0f, ( float )musicInformationDictionary[lFilePath].progress, 0.0f, 1.01f, "progressbar", GUILayout.Width( 132.0f ) );
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
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							
							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

							GUILayout.FlexibleSpace();

							GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
							
							GUILayout.BeginVertical( GUILayout.Width( 80.0f ) );
							{
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							
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
			if( PoolFilePath.GetTimeStampTicks( directoryInfo ) != timeStampTicks )
			{
				timeStampTicks = PoolFilePath.GetTimeStampTicks( directoryInfo );
				string[] lPathArray = PoolFilePath.Get( directoryInfo );

				filePathList = new List<string>();

				for( int i = 0; i < lPathArray.Length; i++ )
				{
					string lFilePath = lPathArray[i];
					Logger.BreakDebug( "Input:" + lFilePath );
					long timeStampFile = File.GetLastWriteTime( lFilePath ).Ticks;
					
					IMusic lMusic = null;

					if( musicInformationDictionary.ContainsKey( lFilePath ) == false )
					{
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
							filePathList.Add( lFilePath );
							musicInformationDictionary.Add( lFilePath, new InputMusicInformation( timeStampFile, false, lMusic, 0.0d ) );
						}
					}
					else if( timeStampFile != musicInformationDictionary[lFilePath].timeStampTicks )
					{
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
							musicInformationDictionary[lFilePath] = new InputMusicInformation( timeStampFile, false, lMusic, 0.0d );
						}
					}
				}
			}
		}
		
		public void OnApplicationQuit()
		{
			foreach( string l in filePathList )
			{
				if( musicInformationDictionary.ContainsKey( l ) == true )
				{
					musicInformationDictionary[l].isSelected = false;
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
					lIndex = musicInformationDictionary.Count - 1;
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

				if( lIndex >= musicInformationDictionary.Count )
				{
					lIndex = 0;
				}
				
				playMusic( filePathList[lIndex] );
			}
		}
	}
}