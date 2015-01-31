using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;

//using Monoamp.Common.Utility;

namespace Unity.View
{
	public abstract class AViewDrag
	{
        public abstract void Awake();
        public abstract void Start();
        public abstract void Update();
        public abstract void OnGUI();
        public abstract void OnRenderObject();
        public abstract void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSampleRate );
        public abstract void OnApplicationQuit();

		protected Rect rectWindow;
		private Rect rectWindowPre;

		//private Texture2D textureCursorMove;
		private Texture2D textureCursorVirtical;
		private Texture2D textureCursorHorizontal;
		private Texture2D textureCursorRightUpLeftDown;
		private Texture2D textureCursorLeftUpRightDown;

		private bool isOnFrameRectTop;
		private bool isOnFrameRectBottom;
		private bool isOnFrameRectLeft;
		private bool isOnFrameRectRight;

		protected delegate void CloseWindow();
		private CloseWindow closeWindow;

		protected AViewDrag( CloseWindow aCloseWindow, Rect aRectWindow )
		{
            closeWindow = aCloseWindow;
            rectWindow = aRectWindow;
            rectWindowPre = rectWindow;

			//textureCursorMove = ( Texture2D )Resources.Load( "Cursor/Move", typeof( Texture2D ) );
			textureCursorVirtical = ( Texture2D )Resources.Load( "Cursor/Virtical", typeof( Texture2D ) );
			textureCursorHorizontal = ( Texture2D )Resources.Load( "Cursor/Horizontal", typeof( Texture2D ) );
			textureCursorRightUpLeftDown = ( Texture2D )Resources.Load( "Cursor/diagonal45", typeof( Texture2D ) );
			textureCursorLeftUpRightDown = ( Texture2D )Resources.Load( "Cursor/diagonal135", typeof( Texture2D ) );
            
			isOnFrameRectTop = false;
			isOnFrameRectBottom = false;
			isOnFrameRectLeft = false;
			isOnFrameRectRight = false;
		}
		
		protected void ControlWindow()
        {
            GUI.DragWindow( new Rect( 4.0f, 4.0f, rectWindow.width - 8.0f, 20.0f ) );

			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();

				if( GUILayout.Button( "", GuiStyleSet.StyleWindow.buttonMinimize ) == true )
				{
					rectWindow = rectWindowPre;
				}

				if( GUILayout.Button( "", GuiStyleSet.StyleWindow.buttonMaximize ) == true )
				{
					rectWindowPre = rectWindow;
					rectWindow = new Rect( 0, 0, Screen.width, Screen.height );
				}

				if( GUILayout.Button( "", GuiStyleSet.StyleWindow.buttonClose ) == true )
				{
					closeWindow();
				}
			}
			GUILayout.EndHorizontal();
		}

		public void ResizeWindow()
		{
			int lPositionX = ( int )rectWindow.x;
			int lPositionY = ( int )rectWindow.y;
			int lWidth = ( int )rectWindow.width;
			int lHeight = ( int )rectWindow.height;

			Rect lFrameRectTop = new Rect( lPositionX - 2.0f, lPositionY - 2.0f, lWidth + 4.0f, 5.0f );
			Rect lFrameRectBottom = new Rect( lPositionX - 2.0f, lPositionY + lHeight - 3.0f, lWidth + 4.0f, 5.0f );
			Rect lFrameRectLeft = new Rect( lPositionX - 2.0f, lPositionY - 2.0f, 5.0f, lHeight + 4.0f );
			Rect lFrameRectRight = new Rect( lPositionX + lWidth - 3.0f, lPositionY - 2.0f, 5.0f, lHeight + 4.0f );

			if( Input.GetMouseButton( 0 ) == false )
			{
				isOnFrameRectTop = false;
				isOnFrameRectBottom = false;
				isOnFrameRectLeft = false;
				isOnFrameRectRight = false;

				if( lFrameRectTop.Contains( Event.current.mousePosition ) )
				{
					isOnFrameRectTop = true;
				}

				if( lFrameRectBottom.Contains( Event.current.mousePosition ) )
				{
					isOnFrameRectBottom = true;
				}

				if( lFrameRectLeft.Contains( Event.current.mousePosition ) )
				{
					isOnFrameRectLeft = true;
				}

				if( lFrameRectRight.Contains( Event.current.mousePosition ) )
				{
					isOnFrameRectRight = true;
				}
			}
			else
			{
				if( isOnFrameRectTop == true )
				{
					ChangeTopSplitTexture();
				}

				if( isOnFrameRectBottom == true )
				{
					ChangeBottomSplitTexture();
				}

				if( isOnFrameRectLeft == true )
				{
					ChangeLeftSplitTexture();
				}

				if( isOnFrameRectRight == true )
				{
					ChangeRightSplitTexture();
				}

				if( isOnFrameRectTop == false && isOnFrameRectBottom == false && isOnFrameRectLeft == false && isOnFrameRectRight == false )
				{
					//MoveSplitTexture( aCurrent, aEditorWindow, aScale );
				}
			}

			if( isOnFrameRectTop == true )
			{
				Cursor.SetCursor( textureCursorVirtical, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
			}

			if( isOnFrameRectBottom == true )
            {
				Cursor.SetCursor( textureCursorVirtical, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
			}

			if( isOnFrameRectLeft == true )
            {
				Cursor.SetCursor( textureCursorHorizontal, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
			}

			if( isOnFrameRectRight == true )
            {
				Cursor.SetCursor( textureCursorHorizontal, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
			}

			if( ( isOnFrameRectTop == true && isOnFrameRectLeft == true ) || ( isOnFrameRectBottom == true && isOnFrameRectRight == true ) )
            {
				Cursor.SetCursor( textureCursorLeftUpRightDown, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
			}
			
			if( ( isOnFrameRectTop == true && isOnFrameRectRight == true ) || ( isOnFrameRectBottom == true && isOnFrameRectLeft == true ) )
            {
				Cursor.SetCursor( textureCursorRightUpLeftDown, new Vector2( 16.0f, 16.0f ), CursorMode.Auto );
            }
		}

		private void ChangeTopSplitTexture()
		{
			float lPositionY = Event.current.mousePosition.y;

			if( lPositionY < rectWindow.y + rectWindow.height - 20.0f )
			{
				rectWindow.height += rectWindow.y - lPositionY;
				rectWindow.y = lPositionY;
			}
		}

		private void ChangeBottomSplitTexture()
		{
			float lPositionY = Event.current.mousePosition.y;

			if( lPositionY > rectWindow.y + 20.0f )
			{
				rectWindow.height = lPositionY - rectWindow.y + 1.0f;
			}
		}

		private void ChangeLeftSplitTexture()
		{
			float lPositionX = Event.current.mousePosition.x;

			if( lPositionX < rectWindow.x + rectWindow.width - 20.0f )
			{
				rectWindow.width += rectWindow.x - lPositionX;
				rectWindow.x = lPositionX;
			}
		}

		private void ChangeRightSplitTexture()
		{
			float lPositionX = Event.current.mousePosition.x;

			if( lPositionX > rectWindow.x + 20.0f )
			{
				rectWindow.width = lPositionX - rectWindow.x + 1.0f;
			}
		}
	}
}
