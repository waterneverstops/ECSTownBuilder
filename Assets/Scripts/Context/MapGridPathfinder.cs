using System.Collections.Generic;
using TownBuilder.Utils.Extensions;
using UnityEngine;

namespace TownBuilder.Context
{
    public class MapGridPathfinder
    {
        private readonly Vector2Int _stopVector2Int = new(-1, -1);

        private readonly List<Vector2Int> _positionsToCheck = new();
        private readonly List<Vector2Int> _path = new();

        private readonly Dictionary<Vector2Int, float> _costDictionary = new();
        private readonly Dictionary<Vector2Int, float> _priorityDictionary = new();

        private readonly Dictionary<Vector2Int, Vector2Int> _parentsDictionary = new();

        public IReadOnlyList<Vector2Int> Path => _path;

        public List<Vector2Int> GetAStarSearchPath(MapGrid grid, Vector2Int startPosition, Vector2Int endPosition, bool isAgent = false)
        {
            _positionsToCheck.Clear();
            _costDictionary.Clear();
            _priorityDictionary.Clear();
            _parentsDictionary.Clear();

            _positionsToCheck.Add(startPosition);
            _costDictionary.Add(startPosition, 0);
            _priorityDictionary.Add(startPosition, 0);
            _parentsDictionary.Add(startPosition, _stopVector2Int);

            while (_positionsToCheck.Count > 0)
            {
                var current = GetClosestVertex(_positionsToCheck, _priorityDictionary);
                _positionsToCheck.Remove(current);
                if (current.Equals(endPosition)) return GeneratePath(_parentsDictionary, current);

                foreach (var neighbour in grid.GetRoadPathfindingNeighbours(current, isAgent))
                {
                    var newCost = _costDictionary[current] + 1;
                    if (!_costDictionary.ContainsKey(neighbour) || newCost < _costDictionary[neighbour])
                    {
                        _costDictionary[neighbour] = newCost;

                        var priority = newCost + neighbour.ManhattanDistance(endPosition);
                        _positionsToCheck.Add(neighbour);
                        _priorityDictionary[neighbour] = priority;

                        _parentsDictionary[neighbour] = current;
                    }
                }
            }

            return new List<Vector2Int>();
        }

        private List<Vector2Int> GeneratePath(Dictionary<Vector2Int, Vector2Int> parentMap, Vector2Int endState)
        {
            _path.Clear();
            var parent = endState;
            while (parent != _stopVector2Int && parentMap.ContainsKey(parent))
            {
                _path.Add(parent);
                parent = parentMap[parent];
            }

            return _path;
        }

        private Vector2Int GetClosestVertex(List<Vector2Int> list, Dictionary<Vector2Int, float> distanceMap)
        {
            var candidate = list[0];
            foreach (var vertex in list)
                if (distanceMap[vertex] < distanceMap[candidate])
                    candidate = vertex;
            return candidate;
        }
    }
}