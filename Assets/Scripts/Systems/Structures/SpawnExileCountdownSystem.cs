using Leopotam.EcsLite;
using TownBuilder.Components.Characters;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class SpawnExileCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float SettlerExileCooldown = 0.15f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var countdownPool = world.GetPool<ExileSettlerCountdown>();
            ref var component = ref countdownPool.Add(entity);
            component.Countdown = SettlerExileCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var countdownFilter = world.Filter<ExileSettlerCountdown>().End();

            var countdownPool = world.GetPool<ExileSettlerCountdown>();
            var exilePool = world.GetPool<SpawnExiles>();
            
            foreach (var countdownEntity in countdownFilter)
            {
                ref var countdownComponent = ref countdownPool.Get(countdownEntity);
                countdownComponent.Countdown -= Time.deltaTime;
                if (countdownComponent.Countdown <= 0f)
                {
                    countdownComponent.Countdown = SettlerExileCooldown;
                    exilePool.Add(countdownEntity);
                }
            }
        }
    }
}