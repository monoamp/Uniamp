using UnityEngine;

using System;
using System.Collections.Generic;
using System.Threading;

using Monoamp.Common.Struct;
using Monoamp.Boundary;

namespace Unity.View
{
	public class ObjectWaveform
	{
		private Transform transform;
		private MeshFilter meshFilter;

		public ObjectWaveform( Transform aTransform, MeshFilter aMeshFilter )
		{
			transform = aTransform;
			meshFilter = aMeshFilter;
		}

		public void SetLoop( LoopInformation aLoopInformation, int aSampleLength, double scale, double offset )
		{
			int[] lIndices = meshFilter.mesh.GetIndices( 0 );

			int lStart = ( int )( -Screen.width * scale * offset + aLoopInformation.start.sample / aSampleLength * Screen.width * scale );
			int lEnd = ( int )( -Screen.width * scale * offset + aLoopInformation.end.sample / aSampleLength * Screen.width * scale );

			for( int i = 0; i < lIndices.Length / 2; i++ )
			{
				if( i > lStart && i <= lEnd )
				{
					lIndices[i * 2 + 0] = i * 2 + 0;
					lIndices[i * 2 + 1] = i * 2 + 1;
				}
				else
				{
					lIndices[i * 2 + 0] = lIndices[lIndices.Length - 1];
					lIndices[i * 2 + 1] = lIndices[lIndices.Length - 1];
				}
			}

			meshFilter.mesh.SetIndices( lIndices, MeshTopology.Lines, 0 );
			meshFilter.mesh.RecalculateBounds();
		}
		
		public void SetPosition( double aPosition, double scale, double offset )
		{
			int count = ( int )( -Screen.width * offset * scale + aPosition * Screen.width * scale );
			float lBase = ( float )count * ( float )Screen.width / ( ( float )Screen.width + 1.0f );
			transform.position = new Vector3( lBase, transform.position.y, transform.position.z );
		}
	}
}
