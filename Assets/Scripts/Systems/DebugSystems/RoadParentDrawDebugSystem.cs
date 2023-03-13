using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Tags;
using TownBuilder.MonoComponents;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.DebugSystems
{
    public class RoadParentDrawDebugSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float DrawRoadParentOffset = 0.5f;

        private readonly EcsCustomInject<PrefabFactory> _prefabFactoryInjection = default;
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        public void Init(IEcsSystems systems)
        {
            _prefabFactoryInjection.Value.Spawn(_prefabSetupInjection.Value.DebugDrawerPrefab);
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

                var roadFilter = world.Filter<Road>().End();
                var roadPool = world.GetPool<Road>();
                var cellPool = world.GetPool<Cell>();

                foreach (var roadEntity in roadFilter)
                {
                    var position = cellPool.Get(roadEntity).Position;
                    var parent = roadPool.Get(roadEntity).Parent;
                    debugDrawer.DrawDebugString(parent.ToString(),
                        new Vector3(position.x + DrawRoadParentOffset, 0f, position.y + DrawRoadParentOffset), Color.red);
                }
            }
        }
    }
}