using UnityEngine;

namespace TownBuilder.Components.Input
{
    public struct LeftMousePressing : IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}