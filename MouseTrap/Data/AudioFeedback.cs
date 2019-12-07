using System;
using System.Collections.Generic;

namespace MouseTrap.Data
{
	public static class AudioFeedback
	{
		public static Func<ICollection<string>> GetStockSounds = LiveAudioFeedback.GetStockSounds;
		public static Action<string> Play { get; set; } = LiveAudioFeedback.Play;
	}
}
