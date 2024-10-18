using System;
using System.Collections.Generic;

namespace Infinity.Networking.Data {
	public class MatchRealtimeMessages {
		public event Action<IDictionary<string, object>> onRealtimeMessageReceived;

		public void RegisterMessage (IDictionary<string, object> message) {
			onRealtimeMessageReceived?.Invoke(message);
		}
	}
}
