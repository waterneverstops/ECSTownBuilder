using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.Context.MapRoadDisjointSet;

namespace TownBuilder.Systems.RoadDisjointSetSystems
{
    public class ReMergeRoadsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;
        private RoadDisjointSet _roadDisjointSet;
        private MapGridDepthFirstSearcher _depthSearchSearcher;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
            _roadDisjointSet = _levelContextInjection.Value.RoadDisjointSet;
            _depthSearchSearcher = new MapGridDepthFirstSearcher();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var mergeFilter = world.Filter<ReMerge>().Inc<Road>().End();

            var cellPool = world.GetPool<Cell>();
            var refreshAccessPool = world.GetPool<RoadRefreshNeighbourAccess>();

            foreach (var mergeEntity in mergeFilter)
            {
                var startNode = _roadDisjointSet[mergeEntity];
                if (!refreshAccessPool.Has(mergeEntity)) refreshAccessPool.Add(mergeEntity);
                if (startNode.Parent != null) continue;

                startNode.Parent = startNode;

                foreach (var entity in _depthSearchSearcher.GetAllRoadCellsInSubset(_grid, cellPool.Get(mergeEntity).Position))
                {
                    _roadDisjointSet[entity].Parent = startNode;
                    _roadDisjointSet[entity].Size = 1;
                    if (!refreshAccessPool.Has(entity)) refreshAccessPool.Add(entity);
                }
            }
        }
    }
}