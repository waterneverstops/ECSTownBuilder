using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;

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
            var newSpawnedGhostRoadsFilter = world.Filter<NewGridBuilding>().Inc<GhostRoad>().End();

            var refreshPool = world.GetPool<RefreshRoadModel>();
            var cellPool = world.GetPool<Cell>();
            var roadPool = world.GetPool<Road>();
            var ghostPool = world.GetPool<GhostRoad>();

            ProcessRoadFilter(newSpawnedRoadsFilter, refreshPool, cellPool, roadPool, ghostPool);
            ProcessRoadFilter(newSpawnedGhostRoadsFilter, refreshPool, cellPool, roadPool, ghostPool);
        }

        public void ProcessRoadFilter(EcsFilter filter, EcsPool<RefreshRoadModel> refreshPool, EcsPool<Cell> cellPool, EcsPool<Road> roadPool, EcsPool<GhostRoad> ghostPool)
        {
            foreach (var newSpawnedRoad in filter)
            {
                if (!refreshPool.Has(newSpawnedRoad))
                    refreshPool.Add(newSpawnedRoad);

                foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(newSpawnedRoad).Position))
                    if (_grid[neighbourPosition].Unpack(out var world, out var entity))
                    {
                        if (!(roadPool.Has(entity) || ghostPool.Has(entity)) || refreshPool.Has(entity)) continue;

                        refreshPool.Add(entity);
                    }
            }
        }
    }
}