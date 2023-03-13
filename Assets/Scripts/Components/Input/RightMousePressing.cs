using UnityEngine;

namespace TownBuilder.Components.Input
{
    public struct RightMousePressing : IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}