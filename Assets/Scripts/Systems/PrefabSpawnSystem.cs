using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.MonoComponents;

namespace TownBuilder.Systems
{
    public class PrefabSpawnSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<PrefabFactory> _prefabFactoryInjection = default;

        private PrefabFactory _prefabFactory;
        
        public void Init(IEcsSystems systems)
        {
            _prefabFactory = _prefabFactoryInjection.Value;
            
            _prefabFactory.SetWorld(systems.GetWorld());
        }
    }
}