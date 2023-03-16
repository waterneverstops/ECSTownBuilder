using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class HunterWorkSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SpawnOffset = 0.5f; 
        
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        private MapGrid _grid;
        private MapGridPathfinder _gridPathfinder;
        private PrefabSetup _prefabSetup;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
            _prefabSetup = _prefabSetupInjection.Value;
            _gridPathfinder = new MapGridPathfinder();
        }
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var workFilter = world.Filter<HunterWork>().End();
            if (workFilter.GetEntitiesCount() == 0) return;

            var hunterFilter = world.Filter<HunterHut>().Inc<RoadAccess>().Inc<HasWorkforce>().Exc<WorkInProgress>().End();
            var foodFilter = world.Filter<FoodSource>().End();

            var cellPool = world.GetPool<Cell>();
            var workPool = world.GetPool<WorkInProgress>();
            var spawnPool = world.GetPool<SpawnPrefab>();
            var maxStoragePool = world.GetPool<StructureMaxStorage>();
            var storagePool = world.GetPool<StructureStorage>();

            foreach (var foodEntity in foodFilter)
            {
                var endPosition = cellPool.Get(foodEntity).Position;
                
                foreach (var hunterEntity in hunterFilter)
                {
                    if (storagePool.Get(hunterEntity).Food >= maxStoragePool.Get(hunterEntity).MaxFood) continue; 
                    
                    var startPosition = cellPool.Get(hunterEntity).Position;

                    var path = _gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.NonStructures, false);
                    if (path.Count == 0) continue;

                    workPool.Add(hunterEntity);
                    var spawnEntity = world.NewEntity();
                    ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                    spawnComponent.PrefabSpawnData = new PrefabSpawnData
                    {
                        Prefab = _prefabSetup.HunterCharacterPrefab,
                        Position = new Vector3(startPosition.x + SpawnOffset, 0, startPosition.y + SpawnOffset),
                        Rotation = Quaternion.identity
                    };
                }
            }
        }
    }
}