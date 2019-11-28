using System;
using System.Diagnostics;

namespace MouseTrap.Logging
{
	public static class Logger
	{
		[Conditional("DEBUG")]
		public static void Write(string source, string msg)
		{
			Debug.WriteLine($"{DateTime.Now.Ticks} {source}: {msg}");
		}
	}
}
