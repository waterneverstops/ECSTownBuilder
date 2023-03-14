using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class SpawnSettlerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int MaxSettlersEachSpawn = 10;
        
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        private PrefabSetup _prefabSetup;
        
        public void Init(IEcsSystems systems)
        {
            _prefabSetup = _prefabSetupInjection.Value;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var spawnFilter = world.Filter<SpawnSettlers>().End();
            if (spawnFilter.GetEntitiesCount() == 0) return;
            
            var settlersLeft = MaxSettlersEachSpawn;

            var houseFilter = world.Filter<House>().Inc<RequestedSettlers>().End();

            var requestPool = world.GetPool<RequestedSettlers>();
            var awaitPool = world.GetPool<AwaitingSettlers>();
            var spawnPool = world.GetPool<SpawnPrefab>();
            
            foreach (var houseEntity in houseFilter)
            {
                var requestingAmount = requestPool.Get(houseEntity).Amount;
                
                if (!awaitPool.Has(houseEntity))
                {
                    awaitPool.Add(houseEntity);
                }

                ref var awaitComponent = ref awaitPool.Get(houseEntity);

                if (awaitComponent.Amount < requestingAmount)
                {
                    while ((awaitComponent.Amount < requestingAmount) && (settlersLeft > 0) )
                    {
                        var spawnEntity = world.NewEntity();
                        ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                        spawnComponent.PrefabSpawnData = new PrefabSpawnData
                        {
                            Prefab = _prefabSetup.SettlerCharacterPrefab,
                            Position = Vector3.zero,
                            Rotation = Quaternion.identity,
                        };
                        awaitComponent.Amount++;
                        settlersLeft--;
                    }
                }

                if (settlersLeft == 0)
                {
                    return;
                }
            }
        }
    }
}