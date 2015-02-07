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
	public class ComponentLoopSave : IView
	{
		public Rect Rect{ get; set; }

		private Thread threadSave;
		private bool isOnSave;

		private readonly ComponentPlaylist componentPlaylist;

		public ComponentLoopSave( ComponentPlaylist aComponentPlaylist )
		{
			componentPlaylist = aComponentPlaylist;

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
			isOnSave = false;

			if( threadSave != null )
			{
				threadSave.Abort();
			}
		}

		public void OnGUI()
		{
			if( isOnSave == false )
			{
				if( GUILayout.Button( new GUIContent ( "Save Modified Loop", "StyleLoopTool.ButtonSave" ), GuiStyleSet.StyleLoopTool.buttonSave ) == true )
				{
					isOnSave = true;
					threadSave = new Thread( Execute );
					
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
			else
			{
				if( GUILayout.Toggle( true, new GUIContent ( "Stop", "StyleLoopTool.ButtonSearch" ), GuiStyleSet.StyleLoopTool.buttonSearch ) == false )
				{
					isOnSave = false;
					threadSave.Abort();
				}
			}
		}

		private void Execute()
		{
			for( int i = 0; i < componentPlaylist.filePathList.Count; i++ )
			{
				string lFilePathOutput = componentPlaylist.filePathList[i];
				
				if( componentPlaylist.musicInformationDictionary.ContainsKey( lFilePathOutput ) == true && componentPlaylist.musicInformationDictionary[lFilePathOutput].isSelected == true )
				{
					LoopInformation lLoopInformation = componentPlaylist.musicInformationDictionary[lFilePathOutput].music.Loop;					
					Logger.BreakDebug( "Loop:" + lLoopInformation.start.sample + ", " + lLoopInformation.end.sample + ", " + lLoopInformation.length.sample );

					LoopSave.Execute( lFilePathOutput, lLoopInformation );
					
					componentPlaylist.musicInformationDictionary[lFilePathOutput].isSelected = false;
				}
			}
			
			isOnSave = false;
		}
	}
}
