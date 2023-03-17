using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class GenerateCourierPathToHunterSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int FoodToSendCourier = 400;

        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;
        private MapGridPathfinder _gridPathfinder;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
            _gridPathfinder = new MapGridPathfinder();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var courierFilter = world.Filter<MarketCourier>().Exc<CourierWithFood>().Exc<Path>().Exc<PathEnd>().End();
            var hunterFilter = world.Filter<HunterHut>().Inc<StructureStorage>().End();

            var pathPool = world.GetPool<Path>();
            var cellPool = world.GetPool<Cell>();
            var parentPool = world.GetPool<ParentStructure>();
            var gameObjectPool = world.GetPool<GameObjectLink>();
            var storagePool = world.GetPool<StructureStorage>();

            foreach (var hunterEntity in hunterFilter)
            {
                if (storagePool.Get(hunterEntity).Food < FoodToSendCourier) continue;
                var endPosition = cellPool.Get(hunterEntity).Position;

                foreach (var courierEntity in courierFilter)
                {
                    ref var pathComponent = ref pathPool.Add(courierEntity);

                    var courierPosition = gameObjectPool.Get(courierEntity).Value.transform.position;
                    var startPosition = new Vector2Int(Mathf.FloorToInt(courierPosition.x), Mathf.FloorToInt(courierPosition.z));
                    
                    pathComponent.Points = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Road, false));

                    if (_grid[startPosition].Unpack(out var packedWorld, out var entity))
                    {
                        ref var parentComponent = ref parentPool.Add(courierEntity);
                        parentComponent.Parent = entity;
                    }
                }
            }
        }
    }
}