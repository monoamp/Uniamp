using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Threading;

using Curan.Common.ApplicationComponent.Sound.LoopTool;

namespace Unity.View
{
	public class ViewLoopSearch : IView
	{
		private Thread thread;
		private bool isOnSearch;

		DataLoopPlaylist dataLoopPlaylist;
		DataLoopInputlist dataLoopInputlist;
		
		public Rect Rect{ get; set; }

		public ViewLoopSearch( DataLoopPlaylist aDataLoopPlaylist, DataLoopInputlist aDataLoopDetector )
		{
			dataLoopPlaylist = aDataLoopPlaylist;
			dataLoopInputlist = aDataLoopDetector;

			isOnSearch = false;
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

		public void OnApplicationQuit()
		{
			isOnSearch = false;

			if( thread != null )
			{
				thread.Abort();
			}
		}

		public void OnGUI()
		{
			GUILayout.BeginHorizontal();
			{
                GUILayout.FlexibleSpace();

				if( isOnSearch == false )
				{
					if( GUILayout.Toggle( false, new GUIContent ( "Start", "StyleLoopTool.ButtonSearch" ), GuiStyleSet.StyleLoopTool.buttonSearch ) == true )
					{
						isOnSearch = true;
						thread = new Thread( Execute );
						thread.Start();
					}
				}
				else
				{
					if( GUILayout.Toggle( true, new GUIContent ( "Stop", "StyleLoopTool.ButtonSearch" ), GuiStyleSet.StyleLoopTool.buttonSearch ) == false )
					{
						isOnSearch = false;
						thread.Abort();
					}
				}
			}
			GUILayout.EndVertical();
		}
		
		private void Execute()
		{
			for( int i = 0; i < dataLoopInputlist.filePathList.Count; i++ )
			{
				if( dataLoopInputlist.isSelectedList[i] == true )
				{
					Debug.Log( "Search:" + dataLoopInputlist.filePathList[i] );
					
					LoopSearchExecutor.Execute( dataLoopInputlist.filePathList[i], dataLoopPlaylist.directoryInfo.FullName + "/" + dataLoopInputlist.filePathList[i], dataLoopInputlist.progressList, i );
					
					dataLoopInputlist.isSelectedList[i] = false;
				}
			}
			
			isOnSearch = false;
		}
	}
}
