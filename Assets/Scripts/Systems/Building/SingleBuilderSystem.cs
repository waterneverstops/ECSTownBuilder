using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;

namespace TownBuilder.Systems.Building
{
    public class SingleBuilderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _mapGrid;
        
        public void Init(IEcsSystems systems)
        {
            _mapGrid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var builderFilter = world.Filter<Builder>().Inc<BuildSingle>().End();
            var pressedFilter = world.Filter<LeftMousePressed>().End();

            foreach (var builderEntity in builderFilter)
            {
                foreach (var pressedEntity in pressedFilter)
                {
                    var pressedPool = world.GetPool<LeftMousePressed>();
                    var position = pressedPool.Get(pressedEntity).Position;

                    if (!_mapGrid.IsPositionInbound(position) || !_mapGrid.IsPositionFree(position)) continue;

                    var builderPool = world.GetPool<Builder>();
                    var prefab = builderPool.Get(builderEntity).Prefab;
                    
                    var spawnEntity = world.NewEntity();
                    var spawnPool = world.GetPool<SpawnPrefabGrid>();
                    ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                    spawnComponent.Position = position;
                    spawnComponent.Prefab = prefab;
                }
            }
        }
    }
}