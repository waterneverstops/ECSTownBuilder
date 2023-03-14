using Leopotam.EcsLite;
using TownBuilder.Components.Structures;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class SpawnSettlerCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SettlerSpawnCooldown = 1f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var countdownPool = world.GetPool<SpawnSettlerCountdown>();
            ref var component = ref countdownPool.Add(entity);
            component.Countdown = SettlerSpawnCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var countdownFilter = world.Filter<SpawnSettlerCountdown>().End();

            var countdownPool = world.GetPool<SpawnSettlerCountdown>();
            var spawnPool = world.GetPool<SpawnSettlers>();
            
            foreach (var countdownEntity in countdownFilter)
            {
                ref var countdownComponent = ref countdownPool.Get(countdownEntity);
                countdownComponent.Countdown -= Time.deltaTime;
                if (countdownComponent.Countdown <= 0f)
                {
                    countdownComponent.Countdown = SettlerSpawnCooldown;
                    spawnPool.Add(countdownEntity);
                }
            }
        }
    }
}