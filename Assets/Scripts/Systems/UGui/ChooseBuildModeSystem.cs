using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using TownBuilder.Components.Building;
using TownBuilder.SO;
using UnityEngine;
using UnityEngine.Scripting;

namespace TownBuilder.Systems.UGui
{
    public sealed class ChooseBuildModeSystem : EcsUguiCallbackSystem, IEcsInitSystem
    {
        private const string BuildRoadWidgetName = "Build_Road";
        private const string BuildHouseWidgetName = "Build_House";
        private const string BuildDestroyWidgetName = "Build_Destroy";

        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        private PrefabSetup _prefabSetup;
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _prefabSetup = _prefabSetupInjection.Value;
            _world = systems.GetWorld();
        }

        [Preserve]
        [EcsUguiClickEvent(BuildRoadWidgetName)]
        private void OnRoadClick(in EcsUguiClickEvent evt)
        {
            ref var builderComponent = ref SetupBuilderOfType<BuildPath>();
            builderComponent.Prefab = _prefabSetup.RoadPrefabSetup.BaseRoadPrefab;
            builderComponent.GhostPrefab = _prefabSetup.RoadPrefabSetup.GhostRoadPrefab;
        }

        [Preserve]
        [EcsUguiClickEvent(BuildHouseWidgetName)]
        private void OnHouseClick(in EcsUguiClickEvent evt)
        {
            Debug.Log("House Check");
        }
        
        [Preserve]
        [EcsUguiClickEvent(BuildDestroyWidgetName)]
        private void OnDestroyClick(in EcsUguiClickEvent evt)
        {
            SetupDestroyer();
        }

        private ref Builder SetupBuilderOfType<T>() where T : struct, IBuilderType
        {
            CleanupBuildComponents();

            var builderPool = _world.GetPool<Builder>();
            var typePool = _world.GetPool<T>();

            var newEntity = _world.NewEntity();
            typePool.Add(newEntity);
            ref var builderComponent = ref builderPool.Add(newEntity);
            return ref builderComponent;
        }

        private void SetupDestroyer()
        {
            CleanupBuildComponents();
            
            var destroyerPool = _world.GetPool<Destroyer>();
            var newEntity = _world.NewEntity();
            destroyerPool.Add(newEntity);
        }

        private void CleanupBuildComponents()
        {
            var builderFilter = _world.Filter<Builder>().End();
            foreach (var builderEntity in builderFilter) _world.DelEntity(builderEntity);
            
            var destroyerFilter = _world.Filter<Destroyer>().End();
            foreach (var destroyerEntity in destroyerFilter) _world.DelEntity(destroyerEntity);
        }
    }
}