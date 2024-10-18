using Infinity.Networking.App;
using Plugins.Infinity.DI.App;
using Plugins.Infinity.DI.Units;
using UnityEngine;

namespace Infinity.Networking.Units {
	[CreateAssetMenu(menuName = "Infinity/Units/NetworkingUnit", fileName = "InfinityNetworkingUnit")]
	public class InfinityNetworkingUnit : AppUnit {
		public override void SetupUnit (AppComponentsRegistry componentsRegistry) {
			base.SetupUnit(componentsRegistry);

			componentsRegistry.Instantiate<MatchmakingApi>();
		}
	}
}
