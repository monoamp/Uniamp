using Monoamp.Common.Struct;

namespace Monoamp.Common.Component.Sound.Player
{
	public interface IPlayer
	{
        double Position{ get; set; }
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
		bool GetFlagPlaying();
		void SetPreviousLoop();
		void SetNextLoop();
		void SetUpLoop();
		void SetDownLoop();
		SoundTime GetTimePosition();
		SoundTime GetTimeElapsed();
		SoundTime GetTimeLength();
		void Update( float[] aSoundBuffer, int aChannels, int aSampleRate );
	}
}
