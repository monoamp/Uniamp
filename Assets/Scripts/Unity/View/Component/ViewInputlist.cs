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
	public class DataLoopInputlist
	{
		public DirectoryInfo directoryInfo;
		public List<string> filePathList;
		public List<double> progressList;
		public List<bool> isSelectedList;
		public List<LoopInformation> loopSetList;
		
		public delegate void PlayMusic( string aFilePath );
		public delegate string GetPlayingMusic();
		
		public PlayMusic playMusic;
		public GetPlayingMusic getPlayingMusic;

		public DataLoopInputlist( DirectoryInfo aDirectoryInfo, PlayMusic aPlayMusic, GetPlayingMusic aGetPlayingMusic )
		{
			directoryInfo = aDirectoryInfo;
			playMusic = aPlayMusic;
			getPlayingMusic = aGetPlayingMusic;
		}
	}

	public class ViewInputlist : IView
    {
		public DataLoopInputlist data;
		private Vector2 scrollPosition;
		private bool isSelectedAll;
		private string[] pathArray;

		private const string STRING_PROGRESS = "Progress";
		
		public Rect Rect{ get; set; }

		public ViewInputlist( DirectoryInfo aDirectoryInfo, DataLoopInputlist.PlayMusic aPlayMusic, DataLoopInputlist.GetPlayingMusic aGetPlayingMusic )
		{
			data = new DataLoopInputlist( aDirectoryInfo, aPlayMusic, aGetPlayingMusic );

			UpdateFileList();
			
			isSelectedAll = false;
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
							for( int i = 0; i < data.isSelectedList.Count; i++ )
							{
								data.isSelectedList[i] = isSelectedAll;
							}
						}
						
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
						GUILayout.Label( new GUIContent( "Name", "StyleTable.LabelHeader" ), GuiStyleSet.StyleTable.labelHeader, GUILayout.MinWidth( 300.0f ) );
						GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );
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

						for( int i = 0; i < data.loopSetList.Count; i++ )
						{
							GUILayout.BeginHorizontal( lViewRow[i % 2] );
							{
								data.isSelectedList[i] = GUILayout.Toggle( data.isSelectedList[i], new GUIContent( "", "StyleGeneral.ToggleCheck" ), GuiStyleSet.StyleGeneral.toggleCheck );
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

								if( data.filePathList[i] == data.getPlayingMusic() )
								{
									if( GUILayout.Toggle( true, new GUIContent( Path.GetFileName( data.filePathList[i] ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == false )
									{
										data.playMusic( data.filePathList[i] );
									}
								}
								else
								{
									if( GUILayout.Toggle( false, new GUIContent( Path.GetFileName( data.filePathList[i] ), "StyleTable.ToggleRow" ), GuiStyleSet.StyleTable.toggleRow, GUILayout.MinWidth( 300.0f ) ) == true )
									{
										data.playMusic( data.filePathList[i] );
									}
								}
								
								GUILayout.Label( new GUIContent( "", "StyleTable.PartitionVertical" ), GuiStyleSet.StyleTable.partitionVertical );

								if( data.isSelectedList[i] == true )
								{
									if( data.progressList[i] > 0.0f )
									{
									GUILayout.HorizontalScrollbar( 0.0f, ( float )data.progressList[i], 0.0f, 1.01f, "progressbar", GUILayout.Width( 132.0f ) );
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
			string[] lPathArray = PoolFilePath.Get( data.directoryInfo );

			if( lPathArray != pathArray )
			{
				pathArray = lPathArray;
                
				data.filePathList = new List<string>();
				data.loopSetList = new List<LoopInformation>();
				data.isSelectedList = new List<bool>();
				data.progressList = new List<double>();

				for( int i = 0; i < pathArray.Length; i++ )
				{
					string extentionLower = Path.GetExtension( pathArray[i] ).ToLower();

					if( extentionLower == ".wav" || extentionLower == ".aif" || extentionLower == ".mp3" || extentionLower == ".ogg" )
                    {
						data.filePathList.Add( pathArray[i] );
						data.loopSetList.Add( new LoopInformation( 44100, 0, 0 ) );
						data.isSelectedList.Add( false );
						data.progressList.Add( 0.0d );
					}
				}
			}
		}
		
		public void OnApplicationQuit()
		{
			for( int i = 0; i < data.isSelectedList.Count; i++ )
			{
				data.isSelectedList[i] = false;
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
					lIndex = data.loopSetList.Count - 1;
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

				if( lIndex >= data.loopSetList.Count )
				{
					lIndex = 0;
				}
				
				data.playMusic( data.filePathList[lIndex] );
			}
		}
	}
}