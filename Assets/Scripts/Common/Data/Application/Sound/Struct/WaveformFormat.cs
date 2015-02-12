using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Common.Struct;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Application.Sound
{
	public struct WaweformFormat
	{
		public readonly int channels;
		public readonly int samples;
		public readonly int sampleRate;
		public readonly int sampleBits;
		
		public WaweformFormat( int aChannels, int aSamples, int aSampleRate, int aSampleBits )
		{
			channels = aChannels;
			samples = aSamples;
			sampleRate = aSampleRate;
			sampleBits = aSampleBits;
		}
	}
}
