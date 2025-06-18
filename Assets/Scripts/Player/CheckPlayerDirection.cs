using UnityEngine;

namespace YSJ
{
    public class CheckPlayerDirection : MonoBehaviour
    {
        public enum Direction
        {
            None = 0,
            Right = 1 << 1,
            Left = 1 << 2,
            Up = 1 << 3,
            Down = 1 << 4,
        }

        [field: SerializeField] public Direction CurrentDirection { get; private set; }
        [field: SerializeField] public float Angle { get; set; }

        public void CheckCurrentDirection()
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
                result |= Direction.Right;
            }
            if (angle >= 45 && angle < 135)
            {
                result |= Direction.Up;
            }
            if ((angle >= 135 && angle < 180) || (angle >= -180 && angle < -135))
            {
                result |= Direction.Left;
            }
            if (angle >= -135 && angle < -45)
            {
                result |= Direction.Down;
            }
            return result;
        }
    }
}