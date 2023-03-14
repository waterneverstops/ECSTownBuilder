using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Context;
using TownBuilder.MonoComponents;

namespace TownBuilder.Systems
{
    public class GridPrefabSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<PrefabFactory> _prefabFactoryInjection = default;

        private PrefabFactory _prefabFactory;
        
        public void Init(IEcsSystems systems)
        {
            _prefabFactory = _prefabFactoryInjection.Value;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var spawnGridFilter = world.Filter<SpawnPrefabGrid>().End();
            var spawnGridPool = world.GetPool<SpawnPrefabGrid>();

            foreach (var entity in spawnGridFilter)
            {
                ref var spawnGridComponent = ref spawnGridPool.Get(entity);
                _prefabFactory.SpawnOnGrid(spawnGridComponent.Prefab, spawnGridComponent.Position);
            }
        }
    }
}