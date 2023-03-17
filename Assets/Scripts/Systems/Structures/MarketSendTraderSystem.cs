using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class MarketSendTraderSystem : IEcsInitSystem, IEcsRunSystem
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

            var workFilter = world.Filter<MarketSendTrader>().End();
            if (workFilter.GetEntitiesCount() == 0) return;

            var marketFilter = world.Filter<Market>().Inc<RoadAccess>().Inc<HasWorkforce>().Inc<MarketSendTrader>().Exc<WorkInProgress>().End();

            var cellPool = world.GetPool<Cell>();
            var workPool = world.GetPool<WorkInProgress>();
            var spawnPool = world.GetPool<SpawnPrefab>();

            foreach (var marketEntity in marketFilter)
            {
                var startPosition = cellPool.Get(marketEntity).Position;

                workPool.Add(marketEntity);
                var spawnEntity = world.NewEntity();
                ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                spawnComponent.PrefabSpawnData = new PrefabSpawnData
                {
                    Prefab = _prefabSetup.MarketTraderCharacterPrefab,
                    Position = new Vector3(startPosition.x + SpawnOffset, 0, startPosition.y + SpawnOffset),
                    Rotation = Quaternion.identity
                };
            }
        }
    }
}