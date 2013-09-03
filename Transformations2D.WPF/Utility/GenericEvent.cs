using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;

namespace Transformations2D.WPF.Utility
{
	public class GenericEvent<TValue> : CompositePresentationEvent<EventParameters<TValue>>
	{
	}

	public class EventParameters<TValue>
	{
		public string Topic { get; private set; }
		public TValue Value { get; private set; }

		public EventParameters(string topic, TValue value)
		{
			Topic = topic;
			Value = value;
		}
	}
}
