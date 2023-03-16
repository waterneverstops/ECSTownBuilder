using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Structures
{
    public class CleanUpExilesSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var pathEndFilter = world.Filter<Exile>().Inc<PathEnd>().End();
            
            var destroyPool = world.GetPool<Destroy>();
            
            foreach (var pathEndEntity in pathEndFilter)
            {
                destroyPool.Add(pathEndEntity);
            }
        }
    }
}