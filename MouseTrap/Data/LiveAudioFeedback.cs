using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.Caching;

namespace MouseTrap.Data
{
	public static class LiveAudioFeedback
	{
		private static readonly MemoryCache SoundCache = MemoryCache.Default;

		private static readonly IDictionary<string, SystemSound> StockSounds = new Dictionary<string, SystemSound>
		{
			{ "None", null },
			{ "Asterisk", SystemSounds.Asterisk },
			{ "Beep", SystemSounds.Beep },
			{ "Exclamation", SystemSounds.Exclamation },
			{ "Hand", SystemSounds.Hand },
			{ "Question", SystemSounds.Question },
		};

		public static ICollection<string> GetStockSounds()
		{
			return StockSounds.Keys;
		}

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
			if (StockSounds.ContainsKey(source))
			{
				Logging.Logger.DebugWrite($"Playing {source}");
				StockSounds[source]?.Play();
				return true;
			}

			return false;
		}

		private static void PlayCustomSound(string source)
		{
			if (GetSoundPlayer(source) is SoundPlayer sound)
			{
				try
				{
					Logging.Logger.DebugWrite($"Playing {source}");
					sound.Play();
				}
				catch (System.ServiceProcess.TimeoutException)
				{
					Logging.Logger.DebugWrite($"Timed out attempting to play {source}");
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
				Logging.Logger.DebugWrite($"Added {source} to cache");

				return result;
			}

			return null;
		}
	}
}
