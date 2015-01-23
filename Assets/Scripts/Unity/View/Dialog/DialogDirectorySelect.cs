using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.IO;

namespace Unity.View
{
    public class DialogDirectorySelect : ADragWindow
	{
		private ViewDirectorySelector viewDirectorySelector;

		public DialogDirectorySelect( ViewDirectorySelector.CloseWindow aCloseWindow, ViewDirectoryTree aViewDirectoryTree, DirectoryInfo aDirectoryInfo )
            : base( null, new Rect( 10.0f, 10.0f, Screen.width / 2.0f, Screen.height * 2.0f / 3.0f ) )
		{
            viewDirectorySelector = new ViewDirectorySelector( aCloseWindow, aViewDirectoryTree, aDirectoryInfo );
		}
		
        public override void OnGUI()
		{
			ResizeWindow();
			rectWindow = GUI.Window( 2, rectWindow, Window, "Select Directory", GuiStyleSet.StyleWindow.window );
			viewDirectorySelector.Rect = rectWindow;
		}

		private void Window( int windowID )
        {
            ControlWindow();
			viewDirectorySelector.OnGUI();
			//GUI.Label( new Rect( 0, 0, rectWindow.width, rectWindow.height ), new GUIContent( GUI.tooltip ), GuiStyleSet.StyleGeneral.tooltip );
		}
		
        public override void Awake()
		{
			
		}
		
        public override void Start()
		{
			
		}
		
        public override void Update()
		{
			
		}
        
        public override void OnRenderObject()
        {
            
        }

        public override void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			
		}
		
        public override void OnApplicationQuit()
		{
			
		}
	}
}
