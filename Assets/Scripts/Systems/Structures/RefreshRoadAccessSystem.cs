using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.Context.MapRoadDisjointSet;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class RefreshRoadAccessSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private RoadDisjointSet _disjointSet;
        private MapGrid _grid;

        public void Init(IEcsSystems systems)
        {
            _disjointSet = _levelContextInjection.Value.RoadDisjointSet;
            _grid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var structureFilter = world.Filter<Structure>().Inc<RoadAccessType>().Inc<RefreshRoadAccess>().End();

            var roadPool = world.GetPool<Road>();
            var cellPool = world.GetPool<Cell>();
            var accessPool = world.GetPool<RoadAccess>();
            var typePool = world.GetPool<RoadAccessType>();

            foreach (var structureEntity in structureFilter)
            {
                var parents = new List<int>();

                var type = typePool.Get(structureEntity).IsTwoCellRadius;

                foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(structureEntity).Position, type))
                    if (_grid[neighbourPosition].Unpack(out var packedWorld, out var entity))
                    {
                        if (!roadPool.Has(entity)) continue;

                        var parent = _disjointSet.FindParent(entity).Entity;
                        if (parents.Contains(parent)) continue;

                        parents.Add(parent);
                    }

                if (parents.Count == 0)
                {
                    Debug.Log("Deleted");
                    if (accessPool.Has(structureEntity)) accessPool.Del(structureEntity);
                }
                else
                {
                    Debug.Log("Updated");
                    if (!accessPool.Has(structureEntity)) accessPool.Add(structureEntity);
                    ref var accessComponent = ref accessPool.Get(structureEntity);
                    accessComponent.SubsetParents = parents;
                }
            }
        }
    }
}