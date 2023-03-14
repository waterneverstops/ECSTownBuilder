﻿using System.Collections.Generic;
using UnityEngine;

namespace TownBuilder.SO
{
    [CreateAssetMenu(fileName = "HouseConfig", menuName = "ScriptableObjects/HouseConfig", order = 0)]
    public class HouseConfig : ScriptableObject
    {
        public int StartLevelForBigVariant = 2;
        
        public List<HouseLevelDescription> LevelDescriptions;
    }
}