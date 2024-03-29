﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.MonoComponents;

namespace TownBuilder.Systems
{
    public class PrefabSpawnSystem : IEcsInitSystem, IEcsRunSystem
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

            var spawnFilter = world.Filter<SpawnPrefab>().End();
            var spawnPool = world.GetPool<SpawnPrefab>();

            foreach (var entity in spawnFilter)
            {
                ref var spawnComponent = ref spawnPool.Get(entity);
                _prefabFactory.Spawn(spawnComponent.PrefabSpawnData);
            }
        }
    }
}