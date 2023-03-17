using System;
using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Structures
{
    public class FoodConsumptionSystem : IEcsRunSystem
    {
        private const int FoodConsumptionAmount = 1;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var workFilter = world.Filter<FoodConsumption>().End();
            if (workFilter.GetEntitiesCount() == 0) return;

            var houseFilter = world.Filter<House>().Inc<RoadAccess>().End();

            var storagePool = world.GetPool<StructureStorage>();

            foreach (var houseEntity in houseFilter)
            {
                ref var storageComponent = ref storagePool.Get(houseEntity);

                if (storageComponent.Food > 0) storageComponent.Food = Math.Max(0, storageComponent.Food - FoodConsumptionAmount);
            }
        }
    }
}