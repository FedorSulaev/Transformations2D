using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;

namespace Transformations2D.WPF.Utility
{
	public static class ServicesFactory
	{
		// Singleton instance of the EventAggregator service
		private static EventAggregator _eventService;

		// Lock (sync) object
		private static readonly object SyncRoot = new object();

		// Factory method
		public static EventAggregator EventService
		{
			get
			{
				// Lock execution thread in case of multi-threaded access
				lock (SyncRoot)
				{
					return _eventService ?? (_eventService = new EventAggregator());
				}
			}
		}
	}
}
