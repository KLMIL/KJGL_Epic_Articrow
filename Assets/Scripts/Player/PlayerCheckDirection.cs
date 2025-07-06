using UnityEngine;

namespace YSJ
{
    /// <summary>
    /// 플레이어 방향을 체크하는 클래스
    /// </summary>
    public class PlayerCheckDirection : MonoBehaviour
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

            if (-45 <= angle && angle < 45)
            {
                result |= Direction.Right;
            }
            if (45 <= angle && angle < 135)
            {
                result |= Direction.Up;
            }
            if ((135 <= angle && angle < 180) || (-180 <= angle && angle < -135))
            {
                result |= Direction.Left;
            }
            if (-135 <= angle && angle < -45)
            {
                result |= Direction.Down;
            }
            return result;
        }
    }
}