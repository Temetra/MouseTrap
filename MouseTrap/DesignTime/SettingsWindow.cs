using System.Collections.Generic;

namespace MouseTrap.DesignTime
{
	public class SettingsWindow : ViewModels.SettingsWindow
	{
		public SettingsWindow()
		{
			SoundSources = new List<string>(Data.AudioFeedback.GetStockSounds());
			ForegroundSource = SoundSources[0];
			BackgroundSource = SoundSources[1];
		}
	}
}
