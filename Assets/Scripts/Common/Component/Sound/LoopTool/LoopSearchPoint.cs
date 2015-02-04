using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Struct;

namespace Monoamp.Common.Component.Sound.LoopTool
{
	public class LoopSearchPoint
	{
		private const int NUMBER_OF_LOOP = 10;

		private int width;
		private int points;
		public int basePoint;
		private List<int> sadMinList;
		public List<int> samePointList;
		private SByte[] sourceSamples;
		private SByte[] compareSamples;

		public LoopSearchPoint( SByte[] aSampleArray, int aWidth, int aBasePoint, int aPoints )
		{
			width = aWidth;
			points = aPoints;
			basePoint = aBasePoint;
			sourceSamples = aSampleArray;
			compareSamples = new SByte[points];
			sadMinList = new List<int>();
			samePointList = new List<int>();

			for( int i = 0; i < points; i++ )
			{
				compareSamples[i] = sourceSamples[basePoint + width * i / points];
			}

			for( int i = 0; i < NUMBER_OF_LOOP; i++ )
			{
				sadMinList.Add( int.MaxValue );
				samePointList.Add( basePoint );
			}
		}

		public void Search( int aIndex, SByte[] aSamplesTarget )
		{
			int lSad = 0;

			for( int i = 0; i < points && lSad <= sadMinList[NUMBER_OF_LOOP - 1]; i++ )
			{
				int lDiff = ( int )compareSamples[i] - ( int )aSamplesTarget[i];
				lSad += lDiff * lDiff;
			}

			if( lSad <= sadMinList[NUMBER_OF_LOOP - 1] )
			{
				for( int i = 0; i < NUMBER_OF_LOOP; i++ )
				{
					if( lSad <= sadMinList[i] )
					{
						sadMinList.Insert( i, lSad );
						samePointList.Insert( i, aIndex - 1 );
						
						sadMinList.RemoveAt( NUMBER_OF_LOOP );
						samePointList.RemoveAt( NUMBER_OF_LOOP );

						break;
					}
				}
			}
		}

		public LoopSearchPoint( SByte[] aSampleArray, int aLength, int aWidth, int aBasePoint, int aPoints, int aMinLoopLength )
		{
			width = aWidth;
			points = aPoints;
			basePoint = aBasePoint;
			sourceSamples = aSampleArray;
			compareSamples = new SByte[points];
			sadMinList = new List<int>();
			samePointList = new List<int>();

			for( int i = 0; i < points; i++ )
			{
				if( i < compareSamples.Length && basePoint + width * i / points < sourceSamples.Length )
				{
					UnityEngine.Debug.Log( basePoint.ToString() );
					//UnityEngine.Debug.Log( width.ToString() );
					UnityEngine.Debug.Log( i.ToString() );
					//UnityEngine.Debug.Log( points.ToString() );
					//UnityEngine.Debug.Log( ( basePoint + width * i / points ).ToString() );
					compareSamples[i] = sourceSamples[basePoint + width * i / points];
				}
				else
				{
					UnityEngine.Debug.LogError( i.ToString() + "/" + compareSamples.Length );
					UnityEngine.Debug.LogError( ( basePoint + width * i / points ).ToString() + "/" + sourceSamples.Length );
				}
			}

			for( int i = 0; i < NUMBER_OF_LOOP; i++ )
			{
				sadMinList.Add( int.MaxValue );
				samePointList.Add( basePoint );
			}

			for( int i = basePoint + aMinLoopLength; i < aLength - width; i++ )
			{
				int lSad = 0;

				for( int j = 0; j < points && lSad <= sadMinList[NUMBER_OF_LOOP - 1]; j++ )
				{
					int lDiff = ( int )compareSamples[j] - ( int )aSampleArray[i + width * j / points];
					lSad += lDiff * lDiff;
				}

				if( lSad <= sadMinList[NUMBER_OF_LOOP - 1] )
				{
					for( int j = 0; j < NUMBER_OF_LOOP; j++ )
					{
						if( lSad <= sadMinList[j] )
						{
							sadMinList.Insert( j, lSad );
							samePointList.Insert( j, i - 1 );

							sadMinList.RemoveAt( NUMBER_OF_LOOP );
							samePointList.RemoveAt( NUMBER_OF_LOOP );

							break;
						}
					}
				}
			}
		}
	}
}
