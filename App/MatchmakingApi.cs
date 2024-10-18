using Apple.GameKit;
using Apple.GameKit.Multiplayer;
using Infinity.Networking.Api;
using Infinity.Networking.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Infinity.Networking.App {
	public class MatchmakingApi : IMatchmakingApi {
		private GKMatch _currentMatch;

		public async Task<MatchDto> GetMatchDto (CancellationToken cancellationToken) {
			return await RequestMatch(cancellationToken)
				.ContinueWith(gkMatch => {
					if (gkMatch.Result == null)
						return new MatchDto {
							players = new NetPlayerDto[] {},
							realtimeMessages = new MatchRealtimeMessages(),
						};

					var match = new MatchDto() {
						players = gkMatch.Result.Players.Select(x => new NetPlayerDto() {
							id = x.GamePlayerId,
							displayName = x.DisplayName,
						}).ToArray(),
						realtimeMessages = new MatchRealtimeMessages(),
					};

					gkMatch.Result.Delegate.DataReceivedForPlayer += OnMatchDataReceived;

					_currentMatch = gkMatch.Result;

					return match;

					void OnMatchDataReceived (byte[] data, GKPlayer forRecipient, GKPlayer fromPlayer) {
						var jsonData = Encoding.UTF8.GetString(data);
						var messageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);

						match.realtimeMessages.RegisterMessage(messageData);
					}
				}, cancellationToken);
		}

		public void SendCurrentMatchMessage (string message) {
			if (_currentMatch != null) {
				SendMatchData(_currentMatch, message);
			}
		}

		public void DisconnectCurrentMatch () {
			if (_currentMatch != null) {
				_currentMatch.Disconnect();
			}
		}

		private void SendMatchData (GKMatch gkMatch, string message) {
			var bytes = Encoding.UTF8.GetBytes(message);

			gkMatch.Send(bytes, GKMatch.GKSendDataMode.Reliable);
		}

		private async Task<GKMatch> RequestMatch (CancellationToken cancellationToken) {
			var request = GKMatchRequest.Init();

			request.MinPlayers = 1;
			request.MaxPlayers = 2;

			try {
				var match = await GKMatchmakerViewController.Request(request);
				return match;
			}
			catch (GameKitException e) {
				Debug.LogException(e);
				return null;
			}
		}
	}
}
