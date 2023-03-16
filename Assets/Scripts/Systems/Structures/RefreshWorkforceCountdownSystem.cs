using Leopotam.EcsLite;
using TownBuilder.Components.Structures;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class RefreshWorkforceCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float RefreshWorkforceCooldown = 1.1f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var countdownPool = world.GetPool<RefreshWorkforceCountdown>();
            ref var component = ref countdownPool.Add(entity);
            component.Countdown = RefreshWorkforceCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var countdownFilter = world.Filter<RefreshWorkforceCountdown>().End();

            var countdownPool = world.GetPool<RefreshWorkforceCountdown>();
            var refreshPool = world.GetPool<RefreshWorkforce>();
            
            foreach (var countdownEntity in countdownFilter)
            {
                ref var countdownComponent = ref countdownPool.Get(countdownEntity);
                countdownComponent.Countdown -= Time.deltaTime;
                if (countdownComponent.Countdown <= 0f)
                {
                    countdownComponent.Countdown = RefreshWorkforceCooldown;
                    refreshPool.Add(countdownEntity);
                }
            }
        }
    }
}