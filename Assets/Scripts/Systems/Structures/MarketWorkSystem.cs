using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Structures
{
    public class MarketWorkSystem : IEcsRunSystem
    {
        private const int FoodToSendTrader = 400;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var workFilter = world.Filter<MarketWork>().End();
            if (workFilter.GetEntitiesCount() == 0) return;

            var marketFilter = world.Filter<Market>().Inc<RoadAccess>().Inc<HasWorkforce>().Exc<WorkInProgress>().End();

            var storagePool = world.GetPool<StructureStorage>();
            var courierPool = world.GetPool<MarketSendCourier>();
            var traderPool = world.GetPool<MarketSendTrader>();

            foreach (var marketEntity in marketFilter)
                if (storagePool.Get(marketEntity).Food >= FoodToSendTrader)
                    traderPool.Add(marketEntity);
                else
                    courierPool.Add(marketEntity);
        }
    }
}