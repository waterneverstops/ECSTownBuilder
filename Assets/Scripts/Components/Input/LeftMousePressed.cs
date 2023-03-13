using UnityEngine;

namespace TownBuilder.Components.Input
{
    public struct LeftMousePressed : IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}