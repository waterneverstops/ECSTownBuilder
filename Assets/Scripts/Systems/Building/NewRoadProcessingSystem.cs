using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Context;

namespace TownBuilder.Systems.Building
{
    public class NewRoadProcessingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var newSpawnedRoadsFilter = world.Filter<NewGridBuilding>().Inc<Road>().End();

            var refreshPool = world.GetPool<RefreshRoadModel>();
            var cellPool = world.GetPool<Cell>();
            var roadPool = world.GetPool<Road>();

            foreach (var newSpawnedRoad in newSpawnedRoadsFilter)
            {
                refreshPool.Add(newSpawnedRoad);
                foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(newSpawnedRoad).Position))
                    if (_grid[neighbourPosition].Unpack(out world, out var entity))
                    {
                        if (!roadPool.Has(entity) || refreshPool.Has(entity)) continue;

                        refreshPool.Add(entity);
                    }
            }
        }
    }
}