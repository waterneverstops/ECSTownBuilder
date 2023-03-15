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
    public class SpawnSettlerSystem : IEcsInitSystem, IEcsRunSystem
    {
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

            var houseFilter = world.Filter<House>().Inc<RequestedSettlers>().End();

            var requestPool = world.GetPool<RequestedSettlers>();
            var allocatedPool = world.GetPool<AllocatedSettlers>();
            var spawnPool = world.GetPool<SpawnPrefab>();

            foreach (var houseEntity in houseFilter)
            {
                var requestingAmount = requestPool.Get(houseEntity).Amount;

                if (!allocatedPool.Has(houseEntity)) allocatedPool.Add(houseEntity);

                ref var allocatedComponent = ref allocatedPool.Get(houseEntity);

                if (allocatedComponent.Amount >= requestingAmount) continue;

                var spawnEntity = world.NewEntity();
                ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                spawnComponent.PrefabSpawnData = new PrefabSpawnData
                {
                    Prefab = _prefabSetup.SettlerCharacterPrefab,
                    Position = Vector3.zero,
                    Rotation = Quaternion.identity
                };
                
                allocatedComponent.Amount++;
                return;
            }
        }
    }
}