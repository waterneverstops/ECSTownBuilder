using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;
using TownBuilder.Context;
using TownBuilder.SO;

namespace TownBuilder.Systems.Building
{
    public class PathBuilderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        private MapGrid _mapGrid;
        private MapGridPathfinder _gridPathfinder;

        public void Init(IEcsSystems systems)
        {
            _mapGrid = _levelContextInjection.Value.MapGrid;
            _gridPathfinder = new MapGridPathfinder();

            var world = systems.GetWorld();
            var builderEntity = world.NewEntity();
            var builderPool = world.GetPool<Builder>();
            var pathPool = world.GetPool<BuildPath>();

            ref var builderComponent = ref builderPool.Add(builderEntity);
            builderComponent.Prefab = _prefabSetupInjection.Value.RoadPrefabSetup.BaseRoadPrefab;

            pathPool.Add(builderEntity);
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var builderFilter = world.Filter<Builder>().Inc<BuildPath>().End();
            var mouseInputFilter = world.Filter<MousePressed>().Inc<MouseReleased>().End();
            
            foreach (var builderEntity in builderFilter)
            {
                foreach (var mouseInputEntity in mouseInputFilter)
                {
                    var pressedPool = world.GetPool<MousePressed>();
                    var releasedPool = world.GetPool<MouseReleased>();
                    var pressedPosition = pressedPool.Get(mouseInputEntity).Position;
                    var releasedPosition = releasedPool.Get(mouseInputEntity).Position;

                    var path = _gridPathfinder.GetAStarSearchPath(_mapGrid, pressedPosition, releasedPosition);
                    if (path.Count == 0) continue;
                    
                    var builderPool = world.GetPool<Builder>();
                    var prefab = builderPool.Get(builderEntity).Prefab;

                    foreach (var buildPosition in path)
                    {
                        var spawnEntity = world.NewEntity();
                        var spawnPool = world.GetPool<SpawnPrefabGrid>();
                        ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                        spawnComponent.Position = buildPosition;
                        spawnComponent.Prefab = prefab;    
                    }
                }
            }
        }
    }
}