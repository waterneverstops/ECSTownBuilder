using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.Components.Tags;
using TownBuilder.Context;
using TownBuilder.Context.MapRoadDisjointSet;
using TownBuilder.MonoComponents;
using UnityEngine;

namespace TownBuilder.Systems.DebugSystems
{
    public class StructureRoadAccessDrawDebugSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float DrawAccessParentOffset = 0.5f;
        
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;
        
        private RoadDisjointSet _roadDisjointSet;
        
        public void Init(IEcsSystems systems)
        {
            _roadDisjointSet = _levelContextInjection.Value.RoadDisjointSet;
        }


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var debugDrawerFilter = world.Filter<DebugTextDrawerTag>().End();
            var gameObjectPool = world.GetPool<GameObjectLink>();

            foreach (var debugEntity in debugDrawerFilter)
            {
                var debugDrawer = gameObjectPool.Get(debugEntity).Value.GetComponent<DebugTextDrawer>();
                if (debugDrawer == null) return;

                var structureFilter = world.Filter<Structure>().Inc<RoadAccess>().End();

                var cellPool = world.GetPool<Cell>();
                var accessPool = world.GetPool<RoadAccess>();

                foreach (var structureEntity in structureFilter)
                {
                    var roadEntities = accessPool.Get(structureEntity).RoadEntities; 
                    if (roadEntities.Count == 0) return;

                    var parents = new List<int>();
                    foreach (var entity in roadEntities)
                    {
                        var parentEntity = _roadDisjointSet.FindParent(entity).Entity;
                        if (parents.Contains(parentEntity)) continue;
                        parents.Add(parentEntity);
                    }
                    var parentNames = string.Join(", ", parents);

                    var position = cellPool.Get(structureEntity).Position;
                    debugDrawer.DrawDebugString(parentNames,
                        new Vector3(position.x + DrawAccessParentOffset, 0f, position.y + DrawAccessParentOffset), Color.red);
                }
            }
        }
    }
}