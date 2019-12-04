using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MouseTrap.Logging
{
	public class EventIdTraceFilter : TraceFilter
	{
		private readonly HashSet<int> _filteredIds;

		public EventIdTraceFilter() : this(null)
		{
		}

		public EventIdTraceFilter(object initializeData)
		{
			if (initializeData is string data)
			{
				var numbers = data.Split(',')
					.Select(item => int.TryParse(item.Trim(), out int num) ? num : new int?())
					.Where(item => item.HasValue)
					.Select(item => item.Value);

				_filteredIds = new HashSet<int>(numbers);
			}
			else
			{
				_filteredIds = new HashSet<int>();
			}
		}

		public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
		{
			return !_filteredIds.Contains(id);
		}
	}
}
