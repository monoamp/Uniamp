using UnityEngine;
using UnityEditor;

using System;
using System.IO;

namespace Uniamp.Editor
{
	public class SavePrefab : EditorWindow
	{
		private static DateTime dateTime;
		private static FileInfo fileInfo;

		[MenuItem( "Window/Graphic/Save Prefab" )]
		public static void Init()
		{
			EditorWindow.GetWindow<SavePrefab>( false, "Save Prefab" );
			dateTime = DateTime.Now;
			fileInfo = new FileInfo( Application.dataPath + "/Resources/Prefab/GuiStyleSet.prefab" );
			
			Debug.LogWarning( fileInfo.FullName );
			//Debug.LogWarning( "persistentDataPath:" + Application.persistentDataPath );
			//Debug.LogWarning( "streamingAssetsPath:" + Application.streamingAssetsPath );
			//Debug.LogWarning( "temporaryCachePath:" + Application.temporaryCachePath );
		}
		
		void OnGUI()
		{
			if( GUILayout.Button( "Save" ) == true )
			{
				AssetDatabase.SaveAssets();
				dateTime = DateTime.Now;
			}
			
			GUILayout.Label( "Path:" + fileInfo.FullName );
			GUILayout.Label( "Last Write Time:" + File.GetLastWriteTime( fileInfo.FullName ).ToString() );
			GUILayout.Label( dateTime.ToString() );
		}
	}
}
