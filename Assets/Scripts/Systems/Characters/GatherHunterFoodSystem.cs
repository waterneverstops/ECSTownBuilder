using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
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
            var parentPool = world.GetPool<ParentStructure>();
            var destroyPool = world.GetPool<Destroy>();
            var workPool = world.GetPool<WorkInProgress>();

            foreach (var hunterEntity in hunterFilter)
            {
                var parentEntity = parentPool.Get(hunterEntity).Parent; 
                
                ref var storageComponent = ref storagePool.Get(parentEntity);
                storageComponent.Food += 100;

                workPool.Del(parentEntity);
                
                destroyPool.Add(hunterEntity);
            }
        }
    }
}