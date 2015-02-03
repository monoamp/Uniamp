using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View
{
	public class DialogSettings : AViewDrag
	{
		private ComponentSettings viewConfigLoopTool;
		
		public DialogSettings( ComponentSettings.CloseWindow aCloseWindow, Dictionary<string, string> aLanguage )
            : base( null, new Rect( 10.0f, 10.0f, Screen.width / 2.0f, Screen.height * 2.0f / 3.0f ) )
		{
			viewConfigLoopTool = new ComponentSettings( aCloseWindow, aLanguage );
		}
        
        public override void OnGUI()
		{
			ResizeWindow();
			rectWindow = GUI.Window( 3, rectWindow, Window, "Config", GuiStyleSet.StyleWindow.window );
			viewConfigLoopTool.Rect = rectWindow;
		}
		
		private void Window( int windowID )
        {
            ControlWindow();
			viewConfigLoopTool.OnGUI();
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

        public override void OnApplicationQuit()
        {
            
        }

        public override void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate )
		{
			
		}
	}
}
