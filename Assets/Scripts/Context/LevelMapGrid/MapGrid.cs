using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using UnityEngine;

namespace TownBuilder.Context.LevelMapGrid
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

        public bool IsPositionInbound(Vector2Int position)
        {
            return IsPositionInbound(position.x, position.y);
        }

        public bool IsPositionFree(Vector2Int position)
        {
            return IsPositionFree(position.x, position.y);
        }

        public bool IsPositionInbound(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

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

        public List<Vector2Int> GetPathfindingNeighbours(Vector2Int position, PathType pathType)
        {
            var neighboursPositions = GetNeighbours(position);
            for (var i = neighboursPositions.Count - 1; i >= 0; i--)
            {
                var neighbourPosition = neighboursPositions[i];

                switch (pathType)
                {
                    case PathType.NonStructures:
                    {
                        if (!IsPositionFreeOfStructures(this[neighbourPosition]))
                            neighboursPositions.Remove(neighbourPosition);
                        break;
                    }
                    case PathType.Road:
                    {
                        if (!IsPositionOnRoad(this[neighbourPosition]))
                            neighboursPositions.Remove(neighbourPosition);
                        break;
                    }
                }
            }

            return neighboursPositions;
        }

        public List<Vector2Int> GetNeighbours(Vector2Int position, bool isTwoCellLength = false)
        {
            var neighbours = new List<Vector2Int>();
            if (position.x > 0) neighbours.Add(new Vector2Int(position.x - 1, position.y));
            if (position.x < Width - 1) neighbours.Add(new Vector2Int(position.x + 1, position.y));
            if (position.y > 0) neighbours.Add(new Vector2Int(position.x, position.y - 1));
            if (position.y < Height - 1) neighbours.Add(new Vector2Int(position.x, position.y + 1));

            if (!isTwoCellLength) return neighbours;
            
            if (position.x > 1) neighbours.Add(new Vector2Int(position.x - 2, position.y));
            if (position.x < Width - 2) neighbours.Add(new Vector2Int(position.x + 2, position.y));
            if (position.y > 1) neighbours.Add(new Vector2Int(position.x, position.y - 2));
            if (position.y < Height - 2) neighbours.Add(new Vector2Int(position.x, position.y + 2));

            return neighbours;
        }

        private bool IsPositionFreeOfStructures(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity)) return !world.GetPool<Structure>().Has(entity);

            return false;
        }

        private bool IsPositionOnRoad(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity)) return world.GetPool<Road>().Has(entity);

            return false;
        }
    }
}