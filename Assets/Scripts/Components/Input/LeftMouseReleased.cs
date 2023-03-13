using UnityEngine;

namespace TownBuilder.Components.Input
{
    public struct LeftMouseReleased : IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}