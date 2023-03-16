using System;
using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Characters
{
    public class GatherHunterFoodSystem : IEcsRunSystem
    {
        private const int FoodFromSingleHunter = 100;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var hunterFilter = world.Filter<Hunter>().Inc<PathEnd>().Inc<ParentStructure>().Inc<HunterWithFood>().End();

            var storagePool = world.GetPool<StructureStorage>();
            var maxStoragePool = world.GetPool<StructureMaxStorage>();
            var parentPool = world.GetPool<ParentStructure>();
            var destroyPool = world.GetPool<Destroy>();
            var workPool = world.GetPool<WorkInProgress>();

            foreach (var hunterEntity in hunterFilter)
            {
                var parentEntity = parentPool.Get(hunterEntity).Parent; 
                
                ref var storageComponent = ref storagePool.Get(parentEntity);
                storageComponent.Food = Math.Min(storageComponent.Food + FoodFromSingleHunter, maxStoragePool.Get(parentEntity).MaxFood);

                workPool.Del(parentEntity);
                
                destroyPool.Add(hunterEntity);
            }
        }
    }
}