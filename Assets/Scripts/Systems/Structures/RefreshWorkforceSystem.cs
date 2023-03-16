using System.Collections.Generic;
using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Structures
{
    public class RefreshWorkforceSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var refreshFilter = world.Filter<RefreshWorkforce>().End();
            if (refreshFilter.GetEntitiesCount() == 0) return;

            var needFilter = world.Filter<NeedWorkforce>().Inc<RoadAccess>().End();
            var houseFilter = world.Filter<House>().Inc<RoadAccess>().End();

            var accessPool = world.GetPool<RoadAccess>();
            var workPool = world.GetPool<HasWorkforce>();

            foreach (var needEntity in needFilter)
            {
                var needParents = accessPool.Get(needEntity).RoadEntities;

                foreach (var houseEntity in houseFilter)
                {
                    var houseParents = accessPool.Get(houseEntity).RoadEntities;

                    if (CompareParents(needParents, houseParents))
                    {
                        if (!workPool.Has(needEntity)) workPool.Add(needEntity);
                        break;
                    }
                }
            }
        }

        private bool CompareParents(List<int> needParents, List<int> houseParents)
        {
            foreach (var houseParent in houseParents)
                if (needParents.Contains(houseParent))
                    return true;

            return false;
        }
    }
}