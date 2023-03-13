using UnityEngine;

namespace TownBuilder.Components.Input
{
    public interface IMouseInput
    {
        public Vector2Int Position { get; set; }
    }
}