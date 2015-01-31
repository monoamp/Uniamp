using UnityEngine;

using System;

namespace Unity.Function.Graphic
{
    public static class Gui
    {
		private static Material material;
		public static Camera camera;

        static Gui()
        {
			material = new Material( Shader.Find( "Unlit/Transparent" ) );
            //material = new Material( Shader.Find( "Diffuse" ) );
            material.hideFlags = HideFlags.HideAndDontSave;
            material.shader.hideFlags = HideFlags.HideAndDontSave;
        }

		public static void DrawUiTexture( Camera aCamera, Rect aRect, GUIStyle aStyle )
		{
			RectOffset lBorder = aStyle.border;
			Texture2D lTexture = aStyle.normal.background;

			float x1 = aRect.x;
			float x2 = aRect.x + lBorder.left;
			float x3 = aRect.x + aRect.width - lBorder.right;
			float x4 = aRect.x + aRect.width;
			
			float y4 = Screen.height - aRect.y;
			float y3 = Screen.height - aRect.y - lBorder.top;
			float y2 = Screen.height - aRect.y - aRect.height + lBorder.bottom;
			float y1 = Screen.height - aRect.y - aRect.height;

			Vector3 vertex11 = aCamera.ScreenToWorldPoint( new Vector3( x1, y1 ) );
			Vector3 vertex21 = aCamera.ScreenToWorldPoint( new Vector3( x2, y1 ) );
			Vector3 vertex31 = aCamera.ScreenToWorldPoint( new Vector3( x3, y1 ) );
			Vector3 vertex41 = aCamera.ScreenToWorldPoint( new Vector3( x4, y1 ) );

			Vector3 vertex12 = aCamera.ScreenToWorldPoint( new Vector3( x1, y2 ) );
			Vector3 vertex22 = aCamera.ScreenToWorldPoint( new Vector3( x2, y2 ) );
			Vector3 vertex32 = aCamera.ScreenToWorldPoint( new Vector3( x3, y2 ) );
			Vector3 vertex42 = aCamera.ScreenToWorldPoint( new Vector3( x4, y2 ) );
			
			Vector3 vertex13 = aCamera.ScreenToWorldPoint( new Vector3( x1, y3 ) );
			Vector3 vertex23 = aCamera.ScreenToWorldPoint( new Vector3( x2, y3 ) );
			Vector3 vertex33 = aCamera.ScreenToWorldPoint( new Vector3( x3, y3 ) );
			Vector3 vertex43 = aCamera.ScreenToWorldPoint( new Vector3( x4, y3 ) );
			
			Vector3 vertex14 = aCamera.ScreenToWorldPoint( new Vector3( x1, y4 ) );
			Vector3 vertex24 = aCamera.ScreenToWorldPoint( new Vector3( x2, y4 ) );
			Vector3 vertex34 = aCamera.ScreenToWorldPoint( new Vector3( x3, y4 ) );
			Vector3 vertex44 = aCamera.ScreenToWorldPoint( new Vector3( x4, y4 ) );
			
			float u1 = 0.0f;
			float u2 = ( float )lBorder.left / ( float )lTexture.width;
			float u3 = 1.0f - ( float )lBorder.right / ( float )lTexture.width;
			float u4 = 1.0f;
			
			float v1 = 0.0f;
			float v2 = ( float )lBorder.top / ( float )lTexture.height;
			float v3 = 1.0f - ( float )lBorder.bottom / ( float )lTexture.height;
			float v4 = 1.0f;
			
			Vector3 texcoord11 = new Vector3( u1, v1, 0.0f );
			Vector3 texcoord21 = new Vector3( u2, v1, 0.0f );
			Vector3 texcoord31 = new Vector3( u3, v1, 0.0f );
			Vector3 texcoord41 = new Vector3( u4, v1, 0.0f );
			
			Vector3 texcoord12 = new Vector3( u1, v2, 0.0f );
			Vector3 texcoord22 = new Vector3( u2, v2, 0.0f );
			Vector3 texcoord32 = new Vector3( u3, v2, 0.0f );
			Vector3 texcoord42 = new Vector3( u4, v2, 0.0f );
			
			Vector3 texcoord13 = new Vector3( u1, v3, 0.0f );
			Vector3 texcoord23 = new Vector3( u2, v3, 0.0f );
			Vector3 texcoord33 = new Vector3( u3, v3, 0.0f );
			Vector3 texcoord43 = new Vector3( u4, v3, 0.0f );
			
			Vector3 texcoord14 = new Vector3( u1, v4, 0.0f );
			Vector3 texcoord24 = new Vector3( u2, v4, 0.0f );
			Vector3 texcoord34 = new Vector3( u3, v4, 0.0f );
			Vector3 texcoord44 = new Vector3( u4, v4, 0.0f );

			material.mainTexture = aStyle.normal.background;
			material.SetPass( 0 );
			
			GL.PushMatrix();
			{
				GL.MultMatrix( Matrix4x4.TRS( Vector3.zero, Quaternion.identity, Vector3.one ) );

				// Top.
				GL.Begin( GL.TRIANGLE_STRIP );
				{
					GL.TexCoord( texcoord11 );
					GL.Vertex( vertex11 );
					GL.TexCoord( texcoord12 );
					GL.Vertex( vertex12 );
					GL.TexCoord( texcoord21 );
					GL.Vertex( vertex21 );
					GL.TexCoord( texcoord22 );
					GL.Vertex( vertex22 );
					GL.TexCoord( texcoord31 );
					GL.Vertex( vertex31 );
					GL.TexCoord( texcoord32 );
					GL.Vertex( vertex32 );
					GL.TexCoord( texcoord41 );
					GL.Vertex( vertex41 );
					GL.TexCoord( texcoord42 );
					GL.Vertex( vertex42 );
				}
				GL.End();

				// Middle.
				GL.Begin( GL.TRIANGLE_STRIP );
				{
					GL.TexCoord( texcoord12 );
					GL.Vertex( vertex12 );
					GL.TexCoord( texcoord13 );
					GL.Vertex( vertex13 );
					GL.TexCoord( texcoord22 );
					GL.Vertex( vertex22 );
					GL.TexCoord( texcoord23 );
					GL.Vertex( vertex23 );
					GL.TexCoord( texcoord32 );
					GL.Vertex( vertex32 );
					GL.TexCoord( texcoord33 );
					GL.Vertex( vertex33 );
					GL.TexCoord( texcoord42 );
					GL.Vertex( vertex42 );
					GL.TexCoord( texcoord43 );
					GL.Vertex( vertex43 );
				}
				GL.End();

				// Bottom.
				GL.Begin( GL.TRIANGLE_STRIP );
				{
					GL.TexCoord( texcoord13 );
					GL.Vertex( vertex13 );
					GL.TexCoord( texcoord14 );
					GL.Vertex( vertex14 );
					GL.TexCoord( texcoord23 );
					GL.Vertex( vertex23 );
					GL.TexCoord( texcoord24 );
					GL.Vertex( vertex24 );
					GL.TexCoord( texcoord33 );
					GL.Vertex( vertex33 );
					GL.TexCoord( texcoord34 );
					GL.Vertex( vertex34 );
					GL.TexCoord( texcoord43 );
					GL.Vertex( vertex43 );
					GL.TexCoord( texcoord44 );
					GL.Vertex( vertex44 );
				}
				GL.End();
			}
			GL.PopMatrix();
		}
		
		public static void DrawSeekBar( Rect aRect, GUIStyle aStyle, float aPositionLoopStart, float aPositionLoopEnd, float aPositionCurrent )
		{
			if( aPositionCurrent < aPositionLoopStart )
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, aPositionCurrent );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aPositionCurrent, aPositionLoopStart );
				DrawSeekBarPartition( aRect, aStyle, aStyle.active, aPositionLoopStart, aPositionLoopEnd );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aPositionLoopEnd, 1.0f );
			}
			else if( aPositionCurrent == aPositionLoopStart )
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, aPositionCurrent );
				DrawSeekBarPartition( aRect, aStyle, aStyle.active, aPositionCurrent, aPositionLoopEnd );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aPositionLoopEnd, 1.0f );
			}
			else if( aPositionCurrent < aPositionLoopEnd )
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, aPositionLoopStart );
				DrawSeekBarPartition( aRect, aStyle, aStyle.onActive, aPositionLoopStart, aPositionCurrent );
				DrawSeekBarPartition( aRect, aStyle, aStyle.active, aPositionCurrent, aPositionLoopEnd );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aPositionLoopEnd, 1.0f );
			}
			else if( aPositionCurrent == aPositionLoopEnd )
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, aPositionLoopStart );
				DrawSeekBarPartition( aRect, aStyle, aStyle.onActive, aPositionLoopStart, aPositionCurrent );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aPositionLoopEnd, 1.0f );
			}
			else
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, aPositionLoopStart );
				DrawSeekBarPartition( aRect, aStyle, aStyle.onActive, aPositionLoopStart, aPositionLoopEnd );
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, aPositionLoopEnd, aPositionCurrent );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aPositionCurrent, 1.0f );
			}
		}
		
		public static void DrawVolumeBar( Rect aRect, GUIStyle aStyle, float aVolume )
		{
			if( aVolume <= 0 )
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, 0.0f, 1.0f );
			}
			else if( aVolume < 1.0f )
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, aVolume );
				DrawSeekBarPartition( aRect, aStyle, aStyle.normal, aVolume, 1.0f );
			}
			else
			{
				DrawSeekBarPartition( aRect, aStyle, aStyle.onNormal, 0.0f, 1.0f );
			}
		}

		public static void DrawSeekBarPartition( Rect aRect, GUIStyle aStyle, GUIStyleState aStyleState, float aPositionStart, float aPositionEnd )
		{
			RectOffset lBorder = aStyle.border;
			Texture2D lTexture = aStyleState.background;
			
			float x1 = aRect.x;
			float x2 = aRect.x + lBorder.left;
			float x3 = aRect.x + aRect.width - lBorder.right;
			float x4 = aRect.x + aRect.width;

			float xS = aRect.x + aRect.width * aPositionStart;
			float xE = aRect.x + aRect.width * aPositionEnd;

			float y4 = Screen.height - aRect.y;
			float y3 = Screen.height - aRect.y - lBorder.top;
			float y2 = Screen.height - aRect.y - aRect.height + lBorder.bottom;
			float y1 = Screen.height - aRect.y - aRect.height;
			
			Vector3 vertexS1 = camera.ScreenToWorldPoint( new Vector3( xS, y1 ) );
			Vector3 vertex21 = camera.ScreenToWorldPoint( new Vector3( x2, y1 ) );
			Vector3 vertex31 = camera.ScreenToWorldPoint( new Vector3( x3, y1 ) );
			Vector3 vertexE1 = camera.ScreenToWorldPoint( new Vector3( xE, y1 ) );
			
			Vector3 vertexS2 = camera.ScreenToWorldPoint( new Vector3( xS, y2 ) );
			Vector3 vertex22 = camera.ScreenToWorldPoint( new Vector3( x2, y2 ) );
			Vector3 vertex32 = camera.ScreenToWorldPoint( new Vector3( x3, y2 ) );
			Vector3 vertexE2 = camera.ScreenToWorldPoint( new Vector3( xE, y2 ) );
			
			Vector3 vertexS3 = camera.ScreenToWorldPoint( new Vector3( xS, y3 ) );
			Vector3 vertex23 = camera.ScreenToWorldPoint( new Vector3( x2, y3 ) );
			Vector3 vertex33 = camera.ScreenToWorldPoint( new Vector3( x3, y3 ) );
			Vector3 vertexE3 = camera.ScreenToWorldPoint( new Vector3( xE, y3 ) );
			
			Vector3 vertexS4 = camera.ScreenToWorldPoint( new Vector3( xS, y4 ) );
			Vector3 vertex24 = camera.ScreenToWorldPoint( new Vector3( x2, y4 ) );
			Vector3 vertex34 = camera.ScreenToWorldPoint( new Vector3( x3, y4 ) );
			Vector3 vertexE4 = camera.ScreenToWorldPoint( new Vector3( xE, y4 ) );
			
			float u1 = 0.0f;
			float u2 = ( float )lBorder.left / ( float )lTexture.width;
			float u3 = 1.0f - ( float )lBorder.right / ( float )lTexture.width;
			float u4 = 1.0f;

			float uS = 0.0f;
			float uE = 1.0f;

			if( xS <= x1 )
			{
				uS = u1;
			}
			else if( xS < x2 )
			{
				// Rate.
				uS = ( ( float )lBorder.left - ( x2 - xS ) ) / ( float )lTexture.width;
			}
			else if( xS <= x3 )
			{
				uS = u2;
			}
			else
			{
				uS = 1.0f - ( ( float )lBorder.right - ( xS - x3 ) ) / ( float )lTexture.width;
			}
			
			if( xE >= x4 )
			{
				uE = u4;
			}
			else if( xE > x3 )
			{
				// Rate.
				uE = 1.0f - ( ( float )lBorder.right - ( xE - x3 ) ) / ( float )lTexture.width;
			}
			else if( xE >= x2 )
			{
				uE = u3;
			}
			else
			{
				uE = ( ( float )lBorder.left - ( x2 - xE ) ) / ( float )lTexture.width;
			}

			float v1 = 0.0f;
			float v2 = ( float )lBorder.top / ( float )lTexture.height;
			float v3 = 1.0f - ( float )lBorder.bottom / ( float )lTexture.height;
			float v4 = 1.0f;
			
			Vector3 texcoordS1 = new Vector3( uS, v1, 0.0f );
			Vector3 texcoord21 = new Vector3( u2, v1, 0.0f );
			Vector3 texcoord31 = new Vector3( u3, v1, 0.0f );
			Vector3 texcoordE1 = new Vector3( uE, v1, 0.0f );
			
			Vector3 texcoordS2 = new Vector3( uS, v2, 0.0f );
			Vector3 texcoord22 = new Vector3( u2, v2, 0.0f );
			Vector3 texcoord32 = new Vector3( u3, v2, 0.0f );
			Vector3 texcoordE2 = new Vector3( uE, v2, 0.0f );
			
			Vector3 texcoordS3 = new Vector3( uS, v3, 0.0f );
			Vector3 texcoord23 = new Vector3( u2, v3, 0.0f );
			Vector3 texcoord33 = new Vector3( u3, v3, 0.0f );
			Vector3 texcoordE3 = new Vector3( uE, v3, 0.0f );
			
			Vector3 texcoordS4 = new Vector3( uS, v4, 0.0f );
			Vector3 texcoord24 = new Vector3( u2, v4, 0.0f );
			Vector3 texcoord34 = new Vector3( u3, v4, 0.0f );
			Vector3 texcoordE4 = new Vector3( uE, v4, 0.0f );
			
			material.mainTexture = aStyleState.background;
			material.SetPass( 0 );
			
			GL.PushMatrix();
			{
				GL.MultMatrix( Matrix4x4.TRS( Vector3.zero, Quaternion.identity, Vector3.one ) );
				
				// Top.
				GL.Begin( GL.TRIANGLE_STRIP );
				{
					GL.TexCoord( texcoordS1 );
					GL.Vertex( vertexS1 );
					GL.TexCoord( texcoordS2 );
					GL.Vertex( vertexS2 );

					if( x2 > xS && x2 < xE )
					{
						GL.TexCoord( texcoord21 );
						GL.Vertex( vertex21 );
						GL.TexCoord( texcoord22 );
						GL.Vertex( vertex22 );
					}

					if( x3 > xS && x3 < xE )
					{
						GL.TexCoord( texcoord31 );
						GL.Vertex( vertex31 );
						GL.TexCoord( texcoord32 );
						GL.Vertex( vertex32 );
					}

					GL.TexCoord( texcoordE1 );
					GL.Vertex( vertexE1 );
					GL.TexCoord( texcoordE2 );
					GL.Vertex( vertexE2 );
				}
				GL.End();
				
				// Middle.
				GL.Begin( GL.TRIANGLE_STRIP );
				{
					GL.TexCoord( texcoordS2 );
					GL.Vertex( vertexS2 );
					GL.TexCoord( texcoordS3 );
					GL.Vertex( vertexS3 );
					
					if( x2 > xS && x2 < xE )
					{
						GL.TexCoord( texcoord22 );
						GL.Vertex( vertex22 );
						GL.TexCoord( texcoord23 );
						GL.Vertex( vertex23 );
					}
					
					if( x3 > xS && x3 < xE )
					{
						GL.TexCoord( texcoord32 );
						GL.Vertex( vertex32 );
						GL.TexCoord( texcoord33 );
						GL.Vertex( vertex33 );
					}
					
					GL.TexCoord( texcoordE2 );
					GL.Vertex( vertexE2 );
					GL.TexCoord( texcoordE3 );
					GL.Vertex( vertexE3 );
				}
				GL.End();
				
				// Bottom.
				GL.Begin( GL.TRIANGLE_STRIP );
				{
					GL.TexCoord( texcoordS3 );
					GL.Vertex( vertexS3 );
					GL.TexCoord( texcoordS4 );
					GL.Vertex( vertexS4 );
					
					if( x2 > xS && x2 < xE )
					{
						GL.TexCoord( texcoord23 );
						GL.Vertex( vertex23 );
						GL.TexCoord( texcoord24 );
						GL.Vertex( vertex24 );
					}
					
					if( x3 > xS && x3 < xE )
					{
						GL.TexCoord( texcoord33 );
						GL.Vertex( vertex33 );
						GL.TexCoord( texcoord34 );
						GL.Vertex( vertex34 );
					}
					
					GL.TexCoord( texcoordE3 );
					GL.Vertex( vertexE3 );
					GL.TexCoord( texcoordE4 );
					GL.Vertex( vertexE4 );
				}
				GL.End();
			}
			GL.PopMatrix();
		}
	}
}
