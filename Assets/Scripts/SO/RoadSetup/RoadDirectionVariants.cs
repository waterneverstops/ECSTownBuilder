using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownBuilder.SO.RoadSetup
{
    [CreateAssetMenu(fileName = "RoadDirectionVariants", menuName = "ScriptableObjects/RoadDirectionVariants", order = 0)]
    public class RoadDirectionVariants : ScriptableObject
    {
        public List<RoadNeighborsAndVariant> Directions;

        public ViewVariant TryGetSuitableViewVariant(RoadNeighborsByDirections roadNeighbors)
        {
            foreach (var direction in Directions)
            {
                if (direction.Neighbors == roadNeighbors)
                {
                    return direction.ViewVariant;
                }
            }

            return null;
        }
    }
}