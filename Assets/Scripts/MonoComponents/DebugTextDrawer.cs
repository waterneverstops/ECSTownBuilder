using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class DebugTextDrawer : MonoBehaviour
    {
        private class DebugTextOrder
        {
            public string Text;
            public Vector3 Position;
            public Color Color;
        }

        private readonly List<DebugTextOrder> _drawOrders = new();

        public void DrawDebugString(string text, Vector3 worldPos, Color? color = null)
        {
            _drawOrders.Add(new DebugTextOrder
            {
                Text = text,
                Position = worldPos,
                Color = color ?? Color.black
            });
        }

        private void OnDrawGizmos()
        {
            foreach (var order in _drawOrders)
            {
                Handles.BeginGUI();

                var restoreColor = GUI.color;

                GUI.color = order.Color;

                Handles.Label(order.Position, order.Text);
                GUI.color = restoreColor;
                Handles.EndGUI();
            }

            _drawOrders.Clear();
        }
    }
}