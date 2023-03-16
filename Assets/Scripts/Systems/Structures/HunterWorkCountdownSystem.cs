using Leopotam.EcsLite;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Structures;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class HunterWorkCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float HunterWorkCooldown = 2f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var countdownPool = world.GetPool<HunterWorkCountdown>();
            ref var component = ref countdownPool.Add(entity);
            component.Countdown = HunterWorkCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var countdownFilter = world.Filter<HunterWorkCountdown>().End();

            var countdownPool = world.GetPool<HunterWorkCountdown>();
            var workPool = world.GetPool<HunterWork>();
            
            foreach (var countdownEntity in countdownFilter)
            {
                ref var countdownComponent = ref countdownPool.Get(countdownEntity);
                countdownComponent.Countdown -= Time.deltaTime;
                if (countdownComponent.Countdown <= 0f)
                {
                    countdownComponent.Countdown = HunterWorkCooldown;
                    workPool.Add(countdownEntity);
                }
            }
        }
    }
}