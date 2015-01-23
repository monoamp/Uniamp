using System;

using UnityEngine;
using UnityEditor;

namespace LayerEditor.Information
{
	public class DebugInformation : EditorWindow
	{
		private Vector2 scrollPosition;

		[MenuItem( "Window/Debug/Debug Information" )]
		public static void Init()
		{
			EditorWindow.GetWindow<DebugInformation>( false, "Debug Information" );
		}

		void OnInspectorUpdate()
		{
			Repaint();
		}

		void OnGUI()
		{
			scrollPosition = GUILayout.BeginScrollView( scrollPosition, false, false );
			{
				GUILayout.BeginVertical( "Box" );
				{
					//GUILayout.Label( "Use Time (100 nsec):" + Player.timeDiff );
					//GUILayout.Label( "Buffer:" + Player.buffer );
					//GUILayout.Label( "Sample Rate:" + AudioSettings.outputSampleRate );
					//GUILayout.Label( "FPS:" + 1.0f / Player.realTimeDiff );
					//GUILayout.Label( "Sound FPS:" + 10000000.0f / ( Player.timeNow - Player.timePre ) );
					GUILayout.Label( "Total Memory:" + GC.GetTotalMemory( false ) / 1048576 + " / " + SystemInfo.systemMemorySize + " MB" );
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndScrollView();
		}
	}
}
