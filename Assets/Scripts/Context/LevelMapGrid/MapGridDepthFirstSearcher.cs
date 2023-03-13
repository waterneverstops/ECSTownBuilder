using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace TownBuilder.Context.LevelMapGrid
{
    public class MapGridDepthFirstSearcher
    {
        private readonly Stack<Vector2Int> _stackForSearch = new();
        private readonly HashSet<int> _visited = new();

        public HashSet<int> GetAllRoadCellsInSubset(MapGrid mapGrid, Vector2Int start)
        {
            _stackForSearch.Clear();
            _visited.Clear();

            foreach (var position in mapGrid.GetPathfindingNeighbours(start, true)) _stackForSearch.Push(position);

            _visited.Add(GetEntity(mapGrid[start]));

            while (_stackForSearch.Count > 0)
            {
                var position = _stackForSearch.Pop();
                var entity = GetEntity(mapGrid[position]);

                if (!_visited.Contains(entity))
                {
                    _visited.Add(entity);
                    foreach (var newPosition in mapGrid.GetPathfindingNeighbours(position, true)) _stackForSearch.Push(newPosition);
                }
            }

            return _visited;
        }

        private int GetEntity(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            packedEntityWithWorld.Unpack(out var world, out var entity);
            return entity;
        }
    }
}