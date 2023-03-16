using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Links;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class GenerateExilePathSystem : IEcsInitSystem, IEcsRunSystem
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

            var exileFilter = world.Filter<Exile>().Exc<PathEnd>().Exc<Path>().End();

            var pathPool = world.GetPool<Path>();
            var gameObjectPool = world.GetPool<GameObjectLink>();

            var endPosition = Vector2Int.zero;

            foreach (var exileEntity in exileFilter)
            {
                ref var pathComponent = ref pathPool.Add(exileEntity);

                var exilePosition = gameObjectPool.Get(exileEntity).Value.transform.position;
                var startPosition = new Vector2Int(Mathf.FloorToInt(exilePosition.x), Mathf.FloorToInt(exilePosition.z));

                pathComponent.Points =
                    new List<Vector2Int>(_gridPathfinder.GetAStarSearchPath(_grid, startPosition, endPosition, PathType.Any));
            }
        }
    }
}