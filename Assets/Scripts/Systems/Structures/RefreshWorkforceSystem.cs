using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.MapRoadDisjointSet;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class RefreshWorkforceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private RoadDisjointSet _disjointSet;

        public void Init(IEcsSystems systems)
        {
            _disjointSet = _levelContextInjection.Value.RoadDisjointSet;
        }
        
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
                var needRoads = accessPool.Get(needEntity).RoadEntities;
                var needParents = GetParents(needRoads);
                
                foreach (var houseEntity in houseFilter)
                {
                    var houseRoads = accessPool.Get(houseEntity).RoadEntities;
                    var houseParents = GetParents(houseRoads);
                    
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

        private List<int> GetParents(List<int> entities)
        {
            var parents = new List<int>();
            
            foreach (var entity in entities)
            {
                var parent = _disjointSet.FindParent(entity).Entity;
                if (!parents.Contains(parent)) parents.Add(parent);
            }

            return parents;
        }
    }
}