using Leopotam.EcsLite;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Structures;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class FoodConsumptionCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float FoodConsumptionCooldown = 2f;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var entity = world.NewEntity();
            var countdownPool = world.GetPool<FoodConsumptionCountdown>();
            ref var component = ref countdownPool.Add(entity);
            component.Countdown = FoodConsumptionCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var countdownFilter = world.Filter<FoodConsumptionCountdown>().End();

            var countdownPool = world.GetPool<FoodConsumptionCountdown>();
            var foodPool = world.GetPool<FoodConsumption>();

            foreach (var countdownEntity in countdownFilter)
            {
                ref var countdownComponent = ref countdownPool.Get(countdownEntity);
                countdownComponent.Countdown -= Time.deltaTime;
                if (countdownComponent.Countdown <= 0f)
                {
                    countdownComponent.Countdown = FoodConsumptionCooldown;
                    foodPool.Add(countdownEntity);
                }
            }
        }
    }
}