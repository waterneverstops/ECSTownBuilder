using System;

namespace TownBuilder.SO
{
    [Serializable]
    public class HouseLevelDescription
    {
        public ViewVariant SmallView;
        public ViewVariant BigView;
        
        public int MaxCapacity;
        public int FoodToUpgrade;
    }
}