using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Context;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class SpawnFoodSourceSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;
        private readonly EcsCustomInject<PrefabSetup> _prefabSetupInjection = default;

        public void Init(IEcsSystems systems)
        {
            var map = _levelContextInjection.Value.MapGrid;

            var position = new Vector2Int(Random.Range(0, map.Width), Random.Range(0, map.Height));

            var world = systems.GetWorld();
            var spawnPool = world.GetPool<SpawnPrefabGrid>();

            var entity = world.NewEntity();
            ref var spawnComponent = ref spawnPool.Add(entity);
            spawnComponent.Position = position;
            spawnComponent.Prefab = _prefabSetupInjection.Value.FoodSource;
        }
    }
}