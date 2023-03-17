using UnityEngine;

namespace TownBuilder.Components.Characters
{
    public struct WanderPath
    {
        public Vector2Int NextStep;
        public int StepsLeft;
        public Vector2Int LastStep;
    }
}