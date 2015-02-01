using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
    public class DialogDirectorySelect : AViewDrag
	{
		private ComponentDirectorySelector componentDirectorySelector;

		public DialogDirectorySelect( ComponentDirectorySelector.CloseWindow aCloseWindow, List<DirectoryInfo> aDirectoryInfoRecentList )
            : base( null, new Rect( 10.0f, 10.0f, Screen.width / 2.0f, Screen.height * 2.0f / 3.0f ) )
		{
			componentDirectorySelector = new ComponentDirectorySelector( aCloseWindow, aDirectoryInfoRecentList );
		}
		
        public override void OnGUI()
		{
			ResizeWindow();
			rectWindow = GUI.Window( 2, rectWindow, Window, "Select Directory", GuiStyleSet.StyleWindow.window );
			componentDirectorySelector.Rect = rectWindow;
		}

		private void Window( int windowID )
        {
            ControlWindow();
			componentDirectorySelector.OnGUI();
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
