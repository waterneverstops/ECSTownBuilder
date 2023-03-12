using System;
using System.Collections.Generic;
using UnityEngine;

namespace TownBuilder.SO.RoadSetup
{
    [CreateAssetMenu(fileName = "RoadPrefabSetup", menuName = "ScriptableObjects/RoadPrefabSetup", order = 0)]
    public class RoadPrefabSetup : ScriptableObject
    {
        public GameObject BaseRoadPrefab;
        public RoadNeighborsAndVariant SpotVariant;
        public RoadNeighborsAndVariant CrossVariant;

        public List<RoadDirectionVariants> OtherVariants;

        public ViewVariant GetSuitableViewVariant(RoadNeighborsByDirections roadNeighbors)
        {
            if (SpotVariant.Neighbors == roadNeighbors)
            {
                return SpotVariant.ViewVariant;
            }
            
            if (CrossVariant.Neighbors == roadNeighbors)
            {
                return CrossVariant.ViewVariant;
            }

            foreach (var variant in OtherVariants)
            {
                var viewVariant = variant.TryGetSuitableViewVariant(roadNeighbors); 
                if ( viewVariant == null) continue;
                return viewVariant;
            }

            return null;
        }
    }
}