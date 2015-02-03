using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Unity.View
{
	public class UiDirectoryTree : IView
	{
		private DirectoryInfo directoryInfoSelf;
		//private DirectoryInfo directoryInfoSelected;

		private readonly UiDirectoryTree root;
		private readonly List<UiDirectoryTree> childList;

		private bool isDisplayChildren;
		private bool haveChildren;

		public DirectoryInfo DirectoryInfoSelected{ get; set; }
		public Rect Rect{ get; set; }

		// ルート用のコンストラクタ.
		public UiDirectoryTree( DirectoryInfo aDirectoryInfo, DirectoryInfo aDirectoryInfoCurrent )
		{
			isDisplayChildren = false;
			haveChildren = false;
			directoryInfoSelf = aDirectoryInfo;
			DirectoryInfoSelected = aDirectoryInfoCurrent;
			root = this;
			childList = new List<UiDirectoryTree>();

			LoadChildren();
		}

		private UiDirectoryTree( UiDirectoryTree aRoot, DirectoryInfo aDirectoryInfo )
		{
			isDisplayChildren = false;
			haveChildren = false;
			directoryInfoSelf = aDirectoryInfo;
			root = aRoot;
			childList = new List<UiDirectoryTree>();

			LoadChildren();
		}

		private void LoadChildren()
		{
			try
			{
				DirectoryInfo[] lDirectoryInfoArray = directoryInfoSelf.GetDirectories( "*", SearchOption.TopDirectoryOnly );

				if( lDirectoryInfoArray.Length > 0 )
				{
					haveChildren = true;
					
					if( root.DirectoryInfoSelected.FullName.IndexOf( directoryInfoSelf.FullName ) == 0 )
					{
						isDisplayChildren = true;
						
						for( int i = 0; i < lDirectoryInfoArray.Length; i++ )
						{
							UiDirectoryTree lViewDirectoryTree = new UiDirectoryTree( root, lDirectoryInfoArray[i] );
							
							childList.Add( lViewDirectoryTree );
						}
					}
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakDebug( "Exception:" + aExpection );
				Logger.BreakDebug( "FullName:" + directoryInfoSelf.FullName );
			}
		}

		public int GetItemCountDisplay()
		{
			int count = 1;

			if( isDisplayChildren == true )
			{
				for( int i = 0; i < childList.Count; i++ )
				{
					count += childList[i].GetItemCountDisplay();
				}
			}

			return count;
		}
		
		public int GetItemPositionDisplay()
		{
			if( isDisplayChildren == true )
			{
				for( int i = 0; i < childList.Count; i++ )
				{
					if( childList[i].isDisplayChildren == true )
					{
						return 1 + i + childList[i].GetItemPositionDisplay();
					}
				}
			}
			
			return 1;
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
		
		public void OnApplicationQuit()
		{
			
		}
		
		public void OnRenderObject()
		{
			
		}
		
		public void OnGUI()
		{
			Toggle();

			if( isDisplayChildren == true )
			{
				if( haveChildren == true )
				{
					if( childList.Count == 0 )
					{
						DirectoryInfo[] lDirectoryInfoArray = directoryInfoSelf.GetDirectories( "*", SearchOption.TopDirectoryOnly );

						for( int i = 0; i < lDirectoryInfoArray.Length; i++ )
						{
							UiDirectoryTree lViewDirectoryTree = new UiDirectoryTree( root, lDirectoryInfoArray[i] );
							
							childList.Add( lViewDirectoryTree );
						}
					}

					GUILayout.BeginHorizontal();
					{
						GUILayout.Space( GuiStyleSet.StyleList.toggleOpenClose.fixedWidth + GuiStyleSet.StyleList.toggleOpenClose.margin.left + GuiStyleSet.StyleList.toggleOpenClose.margin.right );

						GUILayout.BeginVertical();
						{
							for( int i = 0; i < childList.Count; i++ )
							{
								childList[i].OnGUI();
							}
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
			}
		}

		private void Toggle()
		{
			GUILayout.BeginHorizontal();
			{
				if( haveChildren == true )
				{
					isDisplayChildren = GUILayout.Toggle( isDisplayChildren, new GUIContent( "", "StyleList.ToggleOpenClose" ), GuiStyleSet.StyleList.toggleOpenClose );
				}
				else
				{
					GUILayout.Toggle( true, new GUIContent( "", "StyleGeneral.None" ), GuiStyleSet.StyleGeneral.none, GUILayout.Width( GuiStyleSet.StyleList.toggleOpenClose.fixedWidth ) );
				}

				if( directoryInfoSelf.FullName == root.DirectoryInfoSelected.FullName )
				{
					GUILayout.Toggle( true, new GUIContent( directoryInfoSelf.Name, "StyleList.ToggleLine " ), GuiStyleSet.StyleList.toggleLine );
				}
				else
				{
					bool isSeledted = GUILayout.Toggle( false, new GUIContent( directoryInfoSelf.Name, "StyleList.ToggleLine " ), GuiStyleSet.StyleList.toggleLine );

					if( isSeledted == true )
					{
						root.DirectoryInfoSelected = directoryInfoSelf;
					}
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}
