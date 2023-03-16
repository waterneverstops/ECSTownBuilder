using Leopotam.EcsLite;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class HunterWorkSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var workFilter = world.Filter<HunterWork>().End();
            if (workFilter.GetEntitiesCount() == 0) return;

            var hunterFilter = world.Filter<HunterHut>().Inc<RoadAccess>().End();

            foreach (var hunterEntity in hunterFilter)
            {
            }
        }
    }
}