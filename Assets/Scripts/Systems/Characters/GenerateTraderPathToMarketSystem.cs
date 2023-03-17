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
    public class GenerateTraderPathToMarketSystem : IEcsInitSystem, IEcsRunSystem
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
            
            var traderFilter = world.Filter<MarketTrader>().Inc<WanderPathEnd>().Inc<ParentStructure>().Exc<Path>().Exc<PathEnd>().End();

            var pathPool = world.GetPool<Path>();
            var wanderPathPool = world.GetPool<WanderPath>();
            var cellPool = world.GetPool<Cell>();
            var endPool = world.GetPool<WanderPathEnd>();
            var parentPool = world.GetPool<ParentStructure>();
            var marketPool = world.GetPool<Market>();
            var destroyPool = world.GetPool<Destroy>();

            foreach (var traderEntity in traderFilter)
            {
                var startPosition = endPool.Get(traderEntity).EndPosition;

                var parent = parentPool.Get(traderEntity).Parent;
                if (!marketPool.Has(parent))
                {
                    destroyPool.Add(traderEntity);
                    continue;
                }
                
                var endPosition = cellPool.Get(parent).Position;

                wanderPathPool.Del(traderEntity);

                var path = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Road, false));
                if (path.Count == 0) path = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Any));
                
                ref var pathComponent = ref pathPool.Add(traderEntity);
                pathComponent.Points = path;

                endPool.Del(traderEntity);
            }
        }
    }
}