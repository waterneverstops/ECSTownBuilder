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
    public class GenerateHunterPathToFoodSystem : IEcsInitSystem, IEcsRunSystem
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

            var hunterFilter = world.Filter<Hunter>().Exc<HunterWithFood>().Exc<Path>().Exc<PathEnd>().End();
            var foodFilter = world.Filter<FoodSource>().End();

            var pathPool = world.GetPool<Path>();
            var cellPool = world.GetPool<Cell>();
            var parentPool = world.GetPool<ParentStructure>();
            var gameObjectPool = world.GetPool<GameObjectLink>();

            foreach (var foodEntity in foodFilter)
            {
                var endPosition = cellPool.Get(foodEntity).Position;

                foreach (var hunterEntity in hunterFilter)
                {
                    ref var pathComponent = ref pathPool.Add(hunterEntity);

                    var hunterPosition = gameObjectPool.Get(hunterEntity).Value.transform.position;
                    var startPosition = new Vector2Int(Mathf.FloorToInt(hunterPosition.x), Mathf.FloorToInt(hunterPosition.z));
                    
                    pathComponent.Points = new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.NonStructures, false));

                    if (_grid[startPosition].Unpack(out var packedWorld, out var entity))
                    {
                        ref var parentComponent = ref parentPool.Add(hunterEntity);
                        parentComponent.Parent = entity;
                    }
                }
            }
        }
    }
}