using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Monoamp.Common.Component.Sound.LoopTool;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Unity.View
{
	public class ComponentLoopSearch : IView
	{
		private Thread thread;
		private bool isOnSearch;

		private DataLoopPlaylist dataLoopPlaylist;
		private DataLoopInputlist dataLoopInputlist;
		private List<string> filePathInputList;
		
		public Rect Rect{ get; set; }

		public ComponentLoopSearch( DataLoopPlaylist aDataLoopPlaylist, DataLoopInputlist aDataLoopDetector, List<string> aFilePathInputList )
		{
			dataLoopPlaylist = aDataLoopPlaylist;
			dataLoopInputlist = aDataLoopDetector;
			filePathInputList = aFilePathInputList;

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

						try
						{
							thread.Start();
						}
						catch( Exception aExpection )
						{
							Logger.BreakError( aExpection.ToString() + ":LoopTool Exception" );
						}
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
			for( int i = 0; i < filePathInputList.Count; i++ )
			{
				string lFilePathInput = filePathInputList[i];

				if( dataLoopInputlist.isSelectedDictionary.ContainsKey( lFilePathInput ) == true && dataLoopInputlist.isSelectedDictionary[lFilePathInput] == true )
				{
					string lFilePathOutput = dataLoopPlaylist.directoryInfo.FullName + "/" + Path.GetFileName( lFilePathInput );
					Debug.Log( "Search:" + lFilePathInput );

					List<LoopInformation> lLoopInformationList = new List<LoopInformation>();
					LoopSearchExecutor.Execute( lFilePathInput, lFilePathOutput, dataLoopInputlist.progressDictionary, lLoopInformationList );

					if( dataLoopPlaylist.loopPointListDictionary.ContainsKey( lFilePathOutput ) == false )
					{
						dataLoopPlaylist.loopPointListDictionary.Add( lFilePathOutput, lLoopInformationList );
					}
					else
					{
						dataLoopPlaylist.loopPointListDictionary[lFilePathOutput] = lLoopInformationList;
					}
					
					dataLoopInputlist.isSelectedDictionary[lFilePathInput] = false;
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
