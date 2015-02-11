using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Component.Sound.LoopTool;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Unity.View.LoopEditor
{
	public class ApplicationLoopEditor : IView
	{
		public Rect Rect{ get; set; }

		public readonly List<DirectoryInfo> directoryInfoRecentInputList;
		public readonly List<DirectoryInfo> directoryInfoRecentOutputList;

		private readonly MenuBar menu;
		private readonly ComponentLoopEditor componentLoopEditor;
		private readonly ComponentInputlist componentInputlist;
		private readonly ComponentPlaylist componentPlaylist;
		private readonly ComponentLoopSearch componentLoopSearch;
		private readonly ComponentLoopSave componentLoopSave;
		private readonly ComponentDirectoryBar componentDirectoryBarInput;
		private readonly ComponentDirectoryBar componentDirectoryBarOutput;

		private string tooltipPrevious;

		public ApplicationLoopEditor( DirectoryInfo aDirectoryInfoInput, DirectoryInfo aDirectoryInfoOutput )
		{
			directoryInfoRecentInputList = new List<DirectoryInfo>();
			directoryInfoRecentOutputList = new List<DirectoryInfo>();
			tooltipPrevious = "";

			try
			{
				using( StreamReader u = new StreamReader( Application.streamingAssetsPath + "/Config/LoopToolInput.ini" ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( Directory.Exists( line ) == true )
						{
							directoryInfoRecentInputList.Add( new DirectoryInfo( line ) );
							
							if( directoryInfoRecentInputList.Count >= 5 )
							{
								break;
							}
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );

				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/LoopToolInput.ini" ) )
				{
					Logger.BreakDebug( "Create LoopToolInput.ini" );
				}
			}
			
			try
			{
				using( StreamReader u = new StreamReader( Application.streamingAssetsPath + "/Config/LoopToolOutput.ini" ) )
				{
					for( string line = u.ReadLine(); line != null; line = u.ReadLine() )
					{
						if( Directory.Exists( line ) == true )
						{
							directoryInfoRecentOutputList.Add( new DirectoryInfo( line ) );
							
							if( directoryInfoRecentOutputList.Count >= 5 )
							{
								break;
							}
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/LoopToolOutput.ini" ) )
				{
					Logger.BreakDebug( "Exception:" + aExpection );
					Logger.BreakDebug( "Create LoopToolOutput.ini" );
				}
			}

			if( directoryInfoRecentInputList.Count == 0 )
			{
				directoryInfoRecentInputList.Add( aDirectoryInfoInput );
			}
			
			if( directoryInfoRecentOutputList.Count == 0 )
			{
				directoryInfoRecentOutputList.Add( aDirectoryInfoOutput );
			}

			menu = new MenuBar( Application.streamingAssetsPath + "/Language/LoopTool/Menu/MenuBar.language", this );
			componentLoopEditor = new ComponentLoopEditor( ChangeMusicPrevious, ChangeMusicNext );
			componentDirectoryBarInput = new ComponentDirectoryBar( SetInput, directoryInfoRecentInputList );
			componentDirectoryBarOutput = new ComponentDirectoryBar( SetOutput, directoryInfoRecentOutputList );

			componentInputlist = new ComponentInputlist( directoryInfoRecentInputList[0], PlayMusic, GetPlayingMusic );
			componentPlaylist = new ComponentPlaylist( directoryInfoRecentOutputList[0], PlayMusic, GetPlayingMusic );

			componentLoopSearch = new ComponentLoopSearch( componentInputlist, componentPlaylist );
			componentLoopSave = new ComponentLoopSave( componentPlaylist );

			SetInput( directoryInfoRecentInputList[0] );
			SetOutput( directoryInfoRecentOutputList[0] );
		}
		
		private void PlayMusic( string aFilePath )
		{
			componentLoopEditor.SetPlayer( aFilePath, componentPlaylist.musicInformationDictionary );
			componentLoopEditor.UpdateMesh();
		}
		
		private string GetPlayingMusic()
		{
			return componentLoopEditor.GetFilePath();
		}
		
		public void SetInput( DirectoryInfo aDirectoryInfo )
		{
			componentInputlist.SetDirectory( aDirectoryInfo );

			for( int i = directoryInfoRecentInputList.Count - 1; i >= 0; i-- )
			{
				if( directoryInfoRecentInputList[i].FullName == aDirectoryInfo.FullName )
				{
					directoryInfoRecentInputList.RemoveAt( i );
				}
			}

			directoryInfoRecentInputList.Insert( 0, aDirectoryInfo );

			if( directoryInfoRecentInputList.Count > 5 )
			{
				directoryInfoRecentInputList.RemoveAt( 5 );
			}

			try
			{
				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/LoopToolInput.ini", false ) )
				{
					foreach( DirectoryInfo l in directoryInfoRecentInputList )
					{
						u.WriteLine( l.FullName );
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
				Logger.BreakDebug( "Failed write LoopToolInput.ini" );
			}
		}
		
		public void SetOutput( DirectoryInfo aDirectoryInfo )
		{
			componentPlaylist.SetDirectory( aDirectoryInfo );

			for( int i = directoryInfoRecentOutputList.Count - 1; i >= 0; i-- )
			{
				if( directoryInfoRecentOutputList[i].FullName == aDirectoryInfo.FullName )
				{
					directoryInfoRecentOutputList.RemoveAt( i );
				}
			}

			directoryInfoRecentOutputList.Insert( 0, aDirectoryInfo );
			
			if( directoryInfoRecentOutputList.Count > 5 )
			{
				directoryInfoRecentOutputList.RemoveAt( 5 );
			}

			try
			{
				using( StreamWriter u = new StreamWriter( Application.streamingAssetsPath + "/Config/LoopToolOutput.ini", false ) )
				{
					foreach( DirectoryInfo l in directoryInfoRecentOutputList )
					{
						u.WriteLine( l.FullName );
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
				Logger.BreakDebug( "Failed write LoopToolOutput.ini" );
			}
		}
		
		public void ChangeMusicPrevious()
		{
			componentInputlist.ChangeMusicPrevious();
			componentPlaylist.ChangeMusicPrevious();
		}
		
		public void ChangeMusicNext()
		{
			componentInputlist.ChangeMusicNext();
			componentPlaylist.ChangeMusicNext();
		}
		
		public void Awake()
		{
			componentLoopEditor.Awake();
		}
		
		public void Start()
		{
			
		}
		
		public void Update()
		{
			componentLoopEditor.Update();
		}
		
		public void OnGUI()
		{
			menu.OnGUI();

			componentLoopEditor.OnGUI();
			
			GUILayout.BeginArea( new Rect( 0.0f, 210.0f, Screen.width, Screen.height - 180.0f ) );
			{
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.BeginVertical( GUILayout.Width( Screen.width / 2.0f ) );
						{
							GUILayout.Label( new GUIContent ( "Input", "StyleLoopTool.LabelInput" ), GuiStyleSet.StyleLoopTool.labelInput );
							GUILayout.Label( new GUIContent ( "", "StyleLoopTool.BackgroundInput" ), GuiStyleSet.StyleLoopTool.backgroundInput );
							componentDirectoryBarInput.OnGUI();
							componentInputlist.OnGUI();
						}
						GUILayout.EndVertical();

						GUILayout.BeginVertical( GUILayout.Width( Screen.width / 2.0f ) );
						{
							GUILayout.Label( new GUIContent ( "Output", "StyleLoopTool.LabelOutput" ), GuiStyleSet.StyleLoopTool.labelOutput );
							GUILayout.Label( new GUIContent ( "", "StyleLoopTool.BackgroundOutput" ), GuiStyleSet.StyleLoopTool.backgroundOutput );
							componentDirectoryBarOutput.OnGUI();
							componentPlaylist.OnGUI();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea( new Rect( 0.0f, 210.0f, Screen.width, 160.0f ) );
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					componentLoopSearch.OnGUI();
					GUILayout.FlexibleSpace();
					componentLoopSave.OnGUI();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();

			if( GUI.tooltip != "" && GUI.tooltip != tooltipPrevious )
			{
				Logger.BreakDebug( GUI.tooltip );
				tooltipPrevious = GUI.tooltip;
			}
		}

		public void OnRenderObject()
		{
			float lHeightMenubar = GuiStyleSet.StyleMenu.bar.fixedHeight;
			componentLoopEditor.Rect = new Rect( 0.0f, lHeightMenubar, Screen.width, Screen.height - lHeightMenubar );
			componentLoopEditor.OnRenderObject();
		}

		public void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			componentLoopEditor.OnAudioFilterRead( aSoundBuffer, aChannels, aSampleRate );
		}
		
		public void OnApplicationQuit()
		{
			componentLoopSearch.OnApplicationQuit();
		}

		public bool GetIsCutLast()
		{
			return LoopSave.IsCutLast;
		}
		
		public void SetIsCutLast( bool aIsCutLast )
		{
			LoopSave.IsCutLast = aIsCutLast;
		}
	}
}
