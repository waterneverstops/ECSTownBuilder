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
    public class GenerateSettlerPathSystem : IEcsInitSystem, IEcsRunSystem
    {
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

            var settlerFilter = world.Filter<Settler>().Exc<Path>().Exc<PathEnd>().End();
            var allocatedFilter = world.Filter<AllocatedSettlers>().End();

            var pathPool = world.GetPool<Path>();
            var allocatedPool = world.GetPool<AllocatedSettlers>();
            var awaitPool = world.GetPool<AwaitingSettlers>();
            var cellPool = world.GetPool<Cell>();

            foreach (var settlerEntity in settlerFilter)
            foreach (var allocatedEntity in allocatedFilter)
            {
                var allocatedAmount = allocatedPool.Get(allocatedEntity).Amount;

                if (!awaitPool.Has(allocatedEntity)) awaitPool.Add(allocatedEntity);

                ref var awaitComponent = ref awaitPool.Get(allocatedEntity);

                if (awaitComponent.Amount >= allocatedAmount) continue;

                awaitComponent.Amount++;
                
                ref var pathComponent = ref pathPool.Add(settlerEntity);
                var startPosition = new Vector2Int(0, 0);
                var endPosition = cellPool.Get(allocatedEntity).Position;
                pathComponent.Points = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Any));
            }
        }
    }
}