using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Context;
using TownBuilder.MonoComponents;

namespace TownBuilder.Systems
{
    public class PrefabSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<PrefabFactory> _prefabFactoryInjection = default;
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private PrefabFactory _prefabFactory;
        
        public void Init(IEcsSystems systems)
        {
            _prefabFactory = _prefabFactoryInjection.Value;
            
            _prefabFactory.Init(systems.GetWorld(), _levelContextInjection.Value.MapGrid);
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