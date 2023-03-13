using UnityEngine;

namespace TownBuilder.Components.Input
{
    public struct RightMousePressed : IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}