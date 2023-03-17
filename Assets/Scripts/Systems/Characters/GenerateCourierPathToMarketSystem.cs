using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class GenerateCourierPathToMarketSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int TakeFoodAmount = 400;
        
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
            
            var courierFilter = world.Filter<MarketCourier>().Inc<PathEnd>().Inc<ParentStructure>().Exc<CourierWithFood>().End();

            var pathPool = world.GetPool<Path>();
            var cellPool = world.GetPool<Cell>();
            var foodPool = world.GetPool<CourierWithFood>();
            var endPool = world.GetPool<PathEnd>();
            var parentPool = world.GetPool<ParentStructure>();
            var marketPool = world.GetPool<Market>();
            var destroyPool = world.GetPool<Destroy>();
            var storagePool = world.GetPool<StructureStorage>();

            foreach (var courierEntity in courierFilter)
            {
                var startPosition = endPool.Get(courierEntity).EndPosition;

                var parent = parentPool.Get(courierEntity).Parent;
                if (!marketPool.Has(parent))
                {
                    destroyPool.Add(courierEntity);
                    continue;
                }
                
                var endPosition = cellPool.Get(parent).Position;

                pathPool.Del(courierEntity);

                var path = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Road, false));
                if (path.Count == 0) path = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Any));
                
                ref var pathComponent = ref pathPool.Add(courierEntity);
                pathComponent.Points = path;

                if (_grid[startPosition].Unpack(out var packedWorld, out var entity))
                {
                    ref var hunterStorage = ref storagePool.Get(entity);
                    var hunterFood = Math.Min(hunterStorage.Food, TakeFoodAmount);
                    hunterStorage.Food = Math.Max(0, hunterStorage.Food - TakeFoodAmount);
                    
                    ref var foodComponent = ref foodPool.Add(courierEntity);
                    foodComponent.Amount = hunterFood;
                }

                endPool.Del(courierEntity);
            }
        }
    }
}