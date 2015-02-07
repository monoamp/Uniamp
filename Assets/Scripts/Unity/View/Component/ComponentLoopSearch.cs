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
		public Rect Rect{ get; set; }

		private Thread threadSearch;
		private bool isOnSearch;

		private readonly ComponentInputlist componentInputlist;
		private readonly ComponentPlaylist componentPlaylist;

		public ComponentLoopSearch( ComponentInputlist aComponentInputlist, ComponentPlaylist aComponentPlaylist )
		{
			componentInputlist = aComponentInputlist;
			componentPlaylist = aComponentPlaylist;

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

			if( threadSearch != null )
			{
				threadSearch.Abort();
			}
		}

		public void OnGUI()
		{
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

					LoopSearch.Execute( lFilePathInput, lFilePathOutput, componentInputlist.musicInformationDictionary[lFilePathInput] );

					componentInputlist.musicInformationDictionary[lFilePathInput].isSelected = false;
				}
			}
			
			isOnSearch = false;
		}
	}
}
