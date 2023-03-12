using System;
using UnityEngine;

namespace TownBuilder.Utils
{
    namespace Extensions
    {
        public static class Vector2IntExtension
        {
            public static float ManhattanDistance(this Vector2Int start, Vector2Int end)
            {
                return Math.Abs(end.x - start.x) + Math.Abs(end.y - start.y);
            }
        }
    }
}