using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using UnityEngine;

namespace TownBuilder.Context
{
    public class MapGrid
    {
        private readonly EcsPackedEntityWithWorld[,] _cells;

        public int Width { get; }

        public int Height { get; }

        public MapGrid(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new EcsPackedEntityWithWorld[Width, Height];
        }

        public EcsPackedEntityWithWorld this[int x, int y]
        {
            get => _cells[x, y];
            set => _cells[x, y] = value;
        }
        
        public EcsPackedEntityWithWorld this[Vector2Int position]
        {
            get => _cells[position.x, position.y];
            set => _cells[position.x, position.y] = value;
        }

        public bool IsPositionInbound(Vector2Int position) => IsPositionInbound(position.x, position.y);
        
        public bool IsPositionInbound(int x, int y) => (x >= 0) && (y >= 0) && (x < Width) && (y < Height);

        public bool IsPositionFree(Vector2Int position) => IsPositionFree(position.x, position.y); 

        public bool IsPositionFree(int x, int y)
        {
            var packedEntityWithWorld = _cells[x, y];
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var roadPool = world.GetPool<Road>();
                var structurePool = world.GetPool<Structure>();

                return !roadPool.Has(entity) && !structurePool.Has(entity);
            }

            return true;
        }
    }
}