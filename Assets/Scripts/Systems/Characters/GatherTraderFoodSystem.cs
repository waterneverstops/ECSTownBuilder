using System;
using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Characters
{
    public class GatherTraderFoodSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var traderFilter = world.Filter<MarketTrader>().Inc<PathEnd>().Inc<ParentStructure>().End();

            var storagePool = world.GetPool<StructureStorage>();
            var maxStoragePool = world.GetPool<StructureMaxStorage>();
            var parentPool = world.GetPool<ParentStructure>();
            var destroyPool = world.GetPool<Destroy>();
            var traderPool = world.GetPool<MarketTrader>();
            var workPool = world.GetPool<WorkInProgress>();

            foreach (var traderEntity in traderFilter)
            {
                var parentEntity = parentPool.Get(traderEntity).Parent; 
                
                ref var storageComponent = ref storagePool.Get(parentEntity);
                storageComponent.Food = Math.Min(storageComponent.Food + traderPool.Get(traderEntity).Food, maxStoragePool.Get(parentEntity).MaxFood);

                workPool.Del(parentEntity);
                
                destroyPool.Add(traderEntity);
            }
        }
    }
}