using UnityEngine;

namespace TownBuilder.Components.Input
{
    public struct RightMouseReleased : IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}