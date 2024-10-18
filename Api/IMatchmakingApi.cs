using Infinity.Networking.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Infinity.Networking.Api {
	public interface IMatchmakingApi {
		Task<MatchDto> GetMatchDto (CancellationToken cancellationToken);
		
		void SendCurrentMatchMessage (string message);

		void DisconnectCurrentMatch ();
	}
}
