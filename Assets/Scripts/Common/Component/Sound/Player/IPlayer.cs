using Monoamp.Common.Struct;

namespace Monoamp.Common.Component.Sound.Player
{
	public interface IPlayer
	{
        double PositionRate{ get; set; }
		float Volume{ get; set; }
		bool IsMute{ get; set; }
		bool IsLoop{ get; set; }

		LoopInformation Loop{ get; }
		int LoopNumberX{ get; }
		int LoopNumberY{ get; }

		void Play();
		void Stop();
		void Pause();
		void Record( string aPath );
		string GetFilePath();
		bool GetFlagPlaying();
		void SetPreviousLoop();
		void SetNextLoop();
		void SetUpLoop();
		void SetDownLoop();
		SoundTime GetTPosition();
		SoundTime GetElapsed();
		SoundTime GetLength();
		int Update( float[] aSoundBuffer, int aChannels, int aSampleRate, int aPositionInBuffer );
	}
}
