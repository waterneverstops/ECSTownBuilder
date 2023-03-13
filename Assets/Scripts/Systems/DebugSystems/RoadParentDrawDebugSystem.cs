using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Tags;
using TownBuilder.Context;
using TownBuilder.Context.MapRoadDisjointSet;
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
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private RoadDisjointSet _roadDisjointSet;
        
        public void Init(IEcsSystems systems)
        {
            _roadDisjointSet = _levelContextInjection.Value.RoadDisjointSet;
            
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

                var roadFilter = world.Filter<Road>().Exc<ReMerge>().Exc<Destroy>().End();
                var cellPool = world.GetPool<Cell>();

                foreach (var roadEntity in roadFilter)
                {
                    var parent = _roadDisjointSet[roadEntity].Parent.Entity;
                    
                    var position = cellPool.Get(roadEntity).Position;
                    debugDrawer.DrawDebugString(parent.ToString(),
                        new Vector3(position.x + DrawRoadParentOffset, 0f, position.y + DrawRoadParentOffset), Color.red);
                }
            }
        }
    }
}