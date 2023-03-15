using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.Components.Tags;
using TownBuilder.MonoComponents;
using UnityEngine;

namespace TownBuilder.Systems.DebugSystems
{
    public class StructureRoadAccessDrawDebugSystem : IEcsRunSystem
    {
        private const float DrawAccessParentOffset = 0.5f;

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
                    if (accessPool.Get(structureEntity).SubsetParents.Count == 0) return;

                    var parentNames = string.Join(", ", accessPool.Get(structureEntity).SubsetParents);

                    var position = cellPool.Get(structureEntity).Position;
                    debugDrawer.DrawDebugString(parentNames,
                        new Vector3(position.x + DrawAccessParentOffset, 0f, position.y + DrawAccessParentOffset), Color.red);
                }
            }
        }
    }
}