using Leopotam.EcsLite;
using TownBuilder.Components.Structures;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class MarketWorkCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float MarketWorkCooldown = 2f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var countdownPool = world.GetPool<MarketWorkCountdown>();
            ref var component = ref countdownPool.Add(entity);
            component.Countdown = MarketWorkCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var countdownFilter = world.Filter<MarketWorkCountdown>().End();

            var countdownPool = world.GetPool<MarketWorkCountdown>();
            var workPool = world.GetPool<MarketWork>();
            
            foreach (var countdownEntity in countdownFilter)
            {
                ref var countdownComponent = ref countdownPool.Get(countdownEntity);
                countdownComponent.Countdown -= Time.deltaTime;
                if (countdownComponent.Countdown <= 0f)
                {
                    countdownComponent.Countdown = MarketWorkCooldown;
                    workPool.Add(countdownEntity);
                }
            }
        }
    }
}