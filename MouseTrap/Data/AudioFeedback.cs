using System;

namespace MouseTrap.Data
{
	public static class AudioFeedback
	{
		public static Action<string> Play { get; set; } = LiveAudioFeedback.Play;
	}
}
