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
		private Thread threadSearch;
		private Thread threadSave;

		private bool isOnSearch;
		private bool isOnSave;

		private ComponentInputlist componentInputlist;
		private ComponentPlaylist componentPlaylist;
		
		public Rect Rect{ get; set; }

		public ComponentLoopSearch( ComponentInputlist aComponentInputlist, ComponentPlaylist aComponentPlaylist )
		{
			componentInputlist = aComponentInputlist;
			componentPlaylist = aComponentPlaylist;

			isOnSearch = false;
			isOnSave = false;
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
			isOnSave = false;

			if( threadSearch != null )
			{
				threadSearch.Abort();
			}
			
			if( threadSave != null )
			{
				threadSave.Abort();
			}
		}

		public void OnGUI()
		{
			GUILayout.BeginHorizontal();
			{
                GUILayout.FlexibleSpace();

				if( isOnSearch == false )
				{
					if( GUILayout.Toggle( false, new GUIContent ( "Start Search", "StyleLoopTool.ButtonSearch" ), GuiStyleSet.StyleLoopTool.buttonSearch ) == true )
					{
						isOnSearch = true;
						threadSearch = new Thread( Execute );

						try
						{
							threadSearch.Start();
						}
						catch( Exception aExpection )
						{
							Logger.BreakError( "Search Exception:" + aExpection.ToString() );
						}
					}
				}
				else
				{
					if( GUILayout.Toggle( true, new GUIContent ( "Stop Search", "StyleLoopTool.ButtonSearch" ), GuiStyleSet.StyleLoopTool.buttonSearch ) == false )
					{
						isOnSearch = false;
						threadSearch.Abort();
					}
				}

				GUILayout.FlexibleSpace();
				
				if( isOnSave == false )
				{
					if( GUILayout.Button( new GUIContent ( "Save Modified Loop", "StyleLoopTool.ButtonSave" ), GuiStyleSet.StyleLoopTool.buttonSave ) == true )
					{
						isOnSave = true;
						threadSave = new Thread( SaveModifiedLoop );
						
						try
						{
							threadSave.Start();
						}
						catch( Exception aExpection )
						{
							Logger.BreakError( "Save Exception:" + aExpection.ToString() );
						}
					}
				}
			}
			GUILayout.EndHorizontal();
		}
		
		private void Execute()
		{
			for( int i = 0; i < componentInputlist.filePathList.Count; i++ )
			{
				string lFilePathInput = componentInputlist.filePathList[i];

				if( componentInputlist.musicInformationDictionary.ContainsKey( lFilePathInput ) == true && componentInputlist.musicInformationDictionary[lFilePathInput].isSelected == true )
				{
					string lFilePathOutput = componentPlaylist.directoryInfo.FullName + "/" + Path.GetFileName( lFilePathInput );
					Logger.BreakDebug( "Search:" + lFilePathInput );

					LoopSearchExecutor.Execute( lFilePathInput, lFilePathOutput, componentInputlist.musicInformationDictionary[lFilePathInput] );

					componentInputlist.musicInformationDictionary[lFilePathInput].isSelected = false;
				}
			}
			
			isOnSearch = false;
		}
		
		private void SaveModifiedLoop()
		{
			for( int i = 0; i < componentPlaylist.filePathList.Count; i++ )
			{
				string lFilePathOutput = componentPlaylist.filePathList[i];
				
				if( componentPlaylist.musicInformationDictionary.ContainsKey( lFilePathOutput ) == true && componentPlaylist.musicInformationDictionary[lFilePathOutput].isSelected == true )
				{
					Logger.BreakDebug( "Save:" + lFilePathOutput );
					
					LoopInformation lLoopInformation = componentPlaylist.musicInformationDictionary[lFilePathOutput].music.Loop;
					
					Logger.BreakDebug( "Loop:" + lLoopInformation.start.sample + ", " + lLoopInformation.end.sample + ", " + lLoopInformation.length.sample );
					LoopSearchExecutor.SaveModifiedLoop( lFilePathOutput, lLoopInformation );
					
					componentPlaylist.musicInformationDictionary[lFilePathOutput].isSelected = false;
				}
			}
			
			isOnSave = false;
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
