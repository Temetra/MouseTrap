using System;
using System.IO;
using System.Media;
using System.Runtime.Caching;

namespace MouseTrap.Data
{
	public static class LiveAudioFeedback
	{
		private static readonly MemoryCache SoundCache = MemoryCache.Default;

		public static void Play(string source)
		{
			if (string.IsNullOrWhiteSpace(source)) return;

			if (PlaySystemSound(source) == false)
			{
				PlayCustomSound(source);
			}
		}

		private static bool PlaySystemSound(string source)
		{
			switch (source)
			{
				case "Asterisk":
					SystemSounds.Asterisk.Play();
					return true;
				case "Beep":
					SystemSounds.Beep.Play();
					return true;
				case "Exclamation":
					SystemSounds.Exclamation.Play();
					return true;
				case "Hand":
					SystemSounds.Hand.Play();
					return true;
				case "Question":
					SystemSounds.Question.Play();
					return true;
				default:
					return false;
			}
		}

		private static void PlayCustomSound(string source)
		{
			if (GetSoundPlayer(source) is SoundPlayer sound)
			{
				try
				{
					sound.Play();
				}
				catch (TimeoutException)
				{
					Logging.Logger.Write($"Timed out attempting to play {source}");
				}
			}
		}

		private static SoundPlayer GetSoundPlayer(string source)
		{
			if (SoundCache[source] is SoundPlayer result)
			{
				return result;
			}
			else if (File.Exists(source))
			{
				result = new SoundPlayer(source);

				CacheItemPolicy policy = new CacheItemPolicy
				{
					SlidingExpiration = TimeSpan.FromMinutes(60)
				};

				SoundCache.Set(source, result, policy);
				Logging.Logger.Write($"Added {source} to cache");

				return result;
			}

			return null;
		}
	}
}
