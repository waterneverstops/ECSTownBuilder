using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;

namespace TownBuilder.Systems.Characters
{
    public class SellTraderFoodSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
        }
    
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var traderFilter = world.Filter<MarketTrader>().Inc<WanderStep>().End();

            var storagePool = world.GetPool<StructureStorage>();
            var maxStoragePool = world.GetPool<StructureMaxStorage>();
            var traderPool = world.GetPool<MarketTrader>();
            var stepPool = world.GetPool<WanderStep>();
            var housePool = world.GetPool<House>();

            foreach (var traderEntity in traderFilter)
            {
                var position = stepPool.Get(traderEntity).Position;

                ref var traderComponent = ref traderPool.Get(traderEntity);
                
                var neighbours = _grid.GetNeighbours(position, true);

                foreach (var neighbourPosition in neighbours)
                {
                    if (_grid[neighbourPosition].Unpack(out var packedWorld, out var entity))
                    {
                        if (!housePool.Has(entity)) continue;

                        ref var storageComponent = ref storagePool.Get(entity);
                        var food = storageComponent.Food;
                        var maxFood = maxStoragePool.Get(entity).MaxFood;
                        if (food >= maxFood) continue;

                        var sellFood = maxFood - food;
                        sellFood = Math.Min(traderComponent.Food, sellFood);

                        traderComponent.Food = Math.Max(0, traderComponent.Food - sellFood);
                        storageComponent.Food = Math.Min(maxFood, storageComponent.Food + sellFood);
                    }
                }
            }
        }
    }
}