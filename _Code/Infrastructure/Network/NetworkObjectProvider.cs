using Fusion;
using Utility;

namespace Infrastructure.Network
{
    public class NetworkObjectProvider : NetworkObjectProviderDefault
    {
        private IDependenciesInjector _dependenciesInjector;

        public void SetDependencies(IDependenciesInjector dependenciesInjector)
        {
            _dependenciesInjector = dependenciesInjector;
        }

        public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context,
            out NetworkObject result)
        {
            NetworkObjectAcquireResult acquireResult = base.AcquirePrefabInstance(runner, context, out result);

            if (acquireResult == NetworkObjectAcquireResult.Success)
            {
                _dependenciesInjector.InjectGameObject(result.gameObject);
            }

            return acquireResult;
        }
    }
}
