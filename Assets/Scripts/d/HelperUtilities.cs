using UnityEngine;

namespace dinhvt.Level18
{
    public static class HelperUtilities
    {
        public static bool IsOverLapping(SpriteRenderer sprite1, SpriteRenderer sprite2)
        {
            bool isOverlappingX = IsOverLappingInterval(sprite1.bounds.min.x, sprite1.bounds.max.x, sprite2.bounds.min.x, sprite2.bounds.max.x);

            bool isOverlappingY = IsOverLappingInterval(sprite1.bounds.min.y, sprite1.bounds.max.y, sprite2.bounds.min.y, sprite2.bounds.max.y);

            if (isOverlappingX && isOverlappingY)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsOverLapping(Collider2D sprite1, Collider2D sprite2)
        {
            bool isOverlappingX = IsOverLappingInterval(sprite1.bounds.min.x, sprite1.bounds.max.x, sprite2.bounds.min.x, sprite2.bounds.max.x);

            bool isOverlappingY = IsOverLappingInterval(sprite1.bounds.min.y, sprite1.bounds.max.y, sprite2.bounds.min.y, sprite2.bounds.max.y);

            if (isOverlappingX && isOverlappingY)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsOverLappingInterval(float imin1, float imax1, float imin2, float imax2)
        {
            if (Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDivisibleByThree(int number)
        {
            return number % 3 == 0;
        }
    }
}
