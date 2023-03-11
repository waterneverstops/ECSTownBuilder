using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Context;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TownBuilder.Systems
{
    public class GridInitializationSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var levelContext = _levelContextInjection.Value;
            var grid = levelContext.MapGrid;

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var entity = world.NewEntity();
                var pool = world.GetPool<Cell>();
                ref var cellComponent = ref pool.Add(entity);
                cellComponent.Position = new Vector2Int(x, y);

                grid[x, y] = world.PackEntityWithWorld(entity);
            }
        }
    }
}