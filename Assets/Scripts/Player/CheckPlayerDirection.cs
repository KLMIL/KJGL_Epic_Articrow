using UnityEngine;

namespace YSJ
{
    public class CheckPlayerDirection : MonoBehaviour
    {
        public enum Direction
        {
            none = 0,
            right = 1 << 1,
            left = 1 << 2,
            up = 1 << 3,
            down = 1 << 4,
        }

        [field: SerializeField] public Direction CurrentDirection { get; private set; }
        [field: SerializeField] public float Angle { get; set; }

        void Update()
        {
            CurrentDirection = CheckDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        }

        Direction CheckDirection(Vector2 value)
        {
            value.Normalize();
            float angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg;
            Angle = angle;
            Direction result = 0;

            if (angle >= -45 && angle < 45)
            {
                result |= Direction.right;
            }
            if (angle >= 45 && angle < 135)
            {
                result |= Direction.up;
            }
            if ((angle >= 135 && angle < 180) || (angle >= -180 && angle < -135))
            {
                result |= Direction.left;
            }
            if (angle >= -135 && angle < -45)
            {
                result |= Direction.down;
            }
            return result;
        }
    }
}