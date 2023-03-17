using System;
using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Characters
{
    public class GatherCourierFoodSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var courierFilter = world.Filter<MarketCourier>().Inc<PathEnd>().Inc<ParentStructure>().Inc<CourierWithFood>().End();

            var storagePool = world.GetPool<StructureStorage>();
            var maxStoragePool = world.GetPool<StructureMaxStorage>();
            var parentPool = world.GetPool<ParentStructure>();
            var destroyPool = world.GetPool<Destroy>();
            var foodPool = world.GetPool<CourierWithFood>();
            var workPool = world.GetPool<WorkInProgress>();

            foreach (var courierEntity in courierFilter)
            {
                var parentEntity = parentPool.Get(courierEntity).Parent; 
                
                ref var storageComponent = ref storagePool.Get(parentEntity);
                storageComponent.Food = Math.Min(storageComponent.Food + foodPool.Get(courierEntity).Amount, maxStoragePool.Get(parentEntity).MaxFood);

                workPool.Del(parentEntity);
                
                destroyPool.Add(courierEntity);
            }
        }
    }
}