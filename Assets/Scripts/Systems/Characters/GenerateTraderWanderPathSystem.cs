using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class GenerateTraderWanderPathSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const int FoodInCourier = 400;
        private const int TraderSteps = 20;
        
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var traderFilter = world.Filter<MarketTrader>().Exc<WanderPath>().Exc<WanderPathEnd>().Exc<Path>().Exc<PathEnd>().End();

            var pathPool = world.GetPool<WanderPath>();
            var traderPool = world.GetPool<MarketTrader>();
            var parentPool = world.GetPool<ParentStructure>();
            var gameObjectPool = world.GetPool<GameObjectLink>();
            var storagePool = world.GetPool<StructureStorage>();

            foreach (var traderEntity in traderFilter)
            {
                ref var pathComponent = ref pathPool.Add(traderEntity);
                pathComponent.StepsLeft = TraderSteps;
                pathComponent.LastStep = new Vector2Int(-1, -1);

                ref var traderComponent = ref traderPool.Get(traderEntity);
                traderComponent.Food = FoodInCourier;

                var courierPosition = gameObjectPool.Get(traderEntity).Value.transform.position;
                var startPosition = new Vector2Int(Mathf.FloorToInt(courierPosition.x), Mathf.FloorToInt(courierPosition.z));
                
                pathComponent.NextStep = startPosition;

                if (_grid[startPosition].Unpack(out var packedWorld, out var entity))
                {
                    ref var parentComponent = ref parentPool.Add(traderEntity);
                    parentComponent.Parent = entity;

                    ref var storageComponent = ref storagePool.Get(entity);
                    storageComponent.Food = Math.Max(0, storageComponent.Food - FoodInCourier);
                }
            }
        }
    }
}