using System;

namespace TownBuilder.SO.RoadSetup
{
    [Serializable]
    public class RoadNeighborsByDirections
    {
        public bool Up;
        public bool Left;
        public bool Down;
        public bool Right;
        
        public static bool operator == (RoadNeighborsByDirections n1, RoadNeighborsByDirections n2)
        {
            if ((object)n1 == null)
                return (object)n2 == null;

            return n1.Equals(n2);
        }

        public static bool operator != (RoadNeighborsByDirections n1, RoadNeighborsByDirections n2)
        {
            return !(n1 == n2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var n2 = (RoadNeighborsByDirections)obj;
            return (Up == n2.Up && Left == n2.Left && Down == n2.Down && Right == n2.Right);
        }

        public override int GetHashCode()
        {
            return (Up, Left, Down, Right).GetHashCode();
        }
    }
}