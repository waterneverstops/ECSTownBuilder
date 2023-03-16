using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class SpawnExileSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SpawnOffset = 0.5f;
        
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        private PrefabSetup _prefabSetup;

        public void Init(IEcsSystems systems)
        {
            _prefabSetup = _prefabSetupInjection.Value;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var spawnFilter = world.Filter<SpawnExiles>().End();
            if (spawnFilter.GetEntitiesCount() == 0) return;

            var houseFilter = world.Filter<House>().Exc<RoadAccess>().End();

            var spawnPool = world.GetPool<SpawnPrefab>();
            var housePool = world.GetPool<House>();
            var cellPool = world.GetPool<Cell>();

            foreach (var houseEntity in houseFilter)
            {
                ref var populationComponent = ref housePool.Get(houseEntity);
                
                if (populationComponent.Population == 0) continue;
                populationComponent.Population--;

                var position = cellPool.Get(houseEntity).Position;
                
                var spawnEntity = world.NewEntity();
                ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                spawnComponent.PrefabSpawnData = new PrefabSpawnData
                {
                    Prefab = _prefabSetup.ExileCharacterPrefab,
                    Position = new Vector3(position.x + SpawnOffset, 0f, position.y + SpawnOffset),
                    Rotation = Quaternion.identity
                };
                return;
            }
        }
    }
}