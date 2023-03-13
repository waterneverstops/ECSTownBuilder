using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Components.RoadDisjointSet;
using TownBuilder.Context;
using TownBuilder.Context.DisjointSet;

namespace TownBuilder.Systems.RoadDisjointSetSystems
{
    public class NewRoadParentProcessingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;
        private RoadDisjointSet _roadDisjointSet;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
            _roadDisjointSet = _levelContextInjection.Value.RoadDisjointSet;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var roadFilter = world.Filter<NewGridBuilding>().Inc<Road>().End();

            var cellPool = world.GetPool<Cell>();
            var roadPool = world.GetPool<Road>();
            var mergePool = world.GetPool<Merge>();
            
            foreach (var newSpawnedRoad in roadFilter)
            {
                _roadDisjointSet.AddNode(newSpawnedRoad);
                
                foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(newSpawnedRoad).Position))
                    if (_grid[neighbourPosition].Unpack(out var packedWorld, out var entity))
                    {
                        if (!roadPool.Has(entity) || mergePool.Has(entity)) continue;

                        mergePool.Add(entity);
                    }
            }
        }
    }
}