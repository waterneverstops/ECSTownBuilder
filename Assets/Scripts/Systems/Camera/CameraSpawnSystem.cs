using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.MonoComponents;
using TownBuilder.SO;

namespace TownBuilder.Systems.Camera
{
    public class CameraSpawnSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;
        private readonly EcsCustomInject<PrefabFactory> _prefabFactoryInjection = default;

        public void Init(IEcsSystems systems)
        {
            _prefabFactoryInjection.Value.Spawn(_prefabSetupInjection.Value.CameraPrefab);
        }
    }
}