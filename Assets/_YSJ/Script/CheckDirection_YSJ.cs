using UnityEngine;

public class CheckDirection_YSJ : MonoBehaviour
{
    Rigidbody2D rb2d;
    public enum Direction 
    {
        none = 0,
        right = 1 << 1,
        left = 1 << 2,
        up = 1 << 3,
        down = 1 << 4,

        rightup = right | up,
        rightdown = right | down,
        leftup = left | up,
        leftdown = left | down,
    }
    public Direction CurrentDirection;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CurrentDirection = CheckDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
    }

    Direction CheckDirection(Vector2 Value)
    {
        Value.Normalize();
        float angle = Mathf.Atan2(Value.y, Value.x) * Mathf.Rad2Deg;
        Direction result = 0;

        if (angle >= - 45 && angle < 45)
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
