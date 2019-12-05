using System;
using System.Diagnostics;

namespace MouseTrap.Logging
{
	public static class Logger
	{
		private static readonly Lazy<TraceSource> _source = new Lazy<TraceSource>(() => new TraceSource("MouseTrap"));

		[Conditional("DEBUG")]
		public static void Write(string msg = "")
		{
			StackFrame frame = new StackFrame(1);
			var method = frame.GetMethod();
			var id = $"{method.DeclaringType.FullName}.{method.Name}".GetHashCode();
			var sep = string.IsNullOrEmpty(msg) ? "" : ": ";
			_source.Value.TraceEvent(TraceEventType.Verbose, id, $"{method.Name}{sep}{msg}");
		}
	}
}
