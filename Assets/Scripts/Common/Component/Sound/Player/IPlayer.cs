using Monoamp.Common.Struct;

namespace Monoamp.Common.Component.Sound.Player
{
	public interface IPlayer
	{
        double Position{ get; set; }
		float Volume{ get; set; }
		bool IsMute{ get; set; }
		bool IsLoop{ get; set; }

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
		LoopInformation GetLoopPoint();
		int GetLoopNumberX();
		int GetLoopNumberY();
		void Update( float[] aSoundBuffer, int aChannels, int aSampleRate );
	}
}
