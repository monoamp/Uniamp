using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.IO;
using System.Threading;

using Monoamp.Common.Component.Sound.LoopTool;

namespace Unity.View
{
	public class ComponentLoopSearch : IView
	{
		private Thread thread;
		private bool isOnSearch;

		DataLoopPlaylist dataLoopPlaylist;
		DataLoopInputlist dataLoopInputlist;
		
		public Rect Rect{ get; set; }

		public ComponentLoopSearch( DataLoopPlaylist aDataLoopPlaylist, DataLoopInputlist aDataLoopDetector )
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

				GUILayout.FlexibleSpace();
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
					
					LoopSearchExecutor.Execute( dataLoopInputlist.filePathList[i], dataLoopPlaylist.directoryInfo.FullName + "/" + Path.GetFileName( dataLoopInputlist.filePathList[i] ), dataLoopInputlist.progressList, i );
					
					dataLoopInputlist.isSelectedList[i] = false;
				}
			}
			
			isOnSearch = false;
		}

		public bool GetIsCutLast()
		{
			return LoopSearchExecutor.IsCutLast;
		}
		
		public void SetIsCutLast( bool aIsCutLast )
		{
			LoopSearchExecutor.IsCutLast = aIsCutLast;
		}
	}
}
