using System.Collections.Generic;

namespace TownBuilder.Context.MapRoadDisjointSet
{
    public class RoadDisjointSet
    {
        private readonly Dictionary<int, Node> _set = new();

        public Node this[int entity]
        {
            get => _set[entity];
            set => _set[entity] = value;
        }

        public void AddNode(int entity)
        {
            if (_set.ContainsKey(entity)) return;

            var node = new Node();
            node.Entity = entity;
            node.Parent = node;
            node.Size = 1;
            _set.Add(entity, node);
        }

        public Node FindParent(Node node)
        {
            while (node.Parent != node)
            {
                node.Parent = node.Parent.Parent;
                node = node.Parent;
            }

            return node;
        }

        public void Merge(int firstEntity, int secondEntity)
        {
            Merge(_set[firstEntity], _set[secondEntity]);
        }

        public void RemoveNode(int entity)
        {
            _set.Remove(entity);
        }

        private void Merge(Node firstNode, Node secondNode)
        {
            firstNode = FindParent(firstNode);
            secondNode = FindParent(secondNode);

            if (firstNode == secondNode) return;

            if (firstNode.Size < secondNode.Size) (firstNode.Parent, secondNode.Parent) = (secondNode.Parent, firstNode.Parent);

            secondNode.Parent = firstNode;
            firstNode.Size = firstNode.Size + secondNode.Size;
        }
    }
}