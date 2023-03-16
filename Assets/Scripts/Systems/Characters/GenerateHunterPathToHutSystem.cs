using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class GenerateHunterPathToHutSystem : IEcsInitSystem, IEcsRunSystem
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
            
            var hunterFilter = world.Filter<Hunter>().Inc<PathEnd>().Inc<ParentStructure>().Exc<HunterWithFood>().End();

            var pathPool = world.GetPool<Path>();
            var cellPool = world.GetPool<Cell>();
            var foodPool = world.GetPool<HunterWithFood>();
            var endPool = world.GetPool<PathEnd>();
            var parentPool = world.GetPool<ParentStructure>();

            foreach (var hunterEntity in hunterFilter)
            {
                var startPosition = endPool.Get(hunterEntity).EndPosition;
                var endPosition = cellPool.Get(parentPool.Get(hunterEntity).Parent).Position;

                pathPool.Del(hunterEntity);

                var path = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.NonStructures, false));
                if (path.Count == 0) path = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Any));
                
                ref var pathComponent = ref pathPool.Add(hunterEntity);
                pathComponent.Points = path;

                foodPool.Add(hunterEntity);
                endPool.Del(hunterEntity);
            }
        }
    }
}