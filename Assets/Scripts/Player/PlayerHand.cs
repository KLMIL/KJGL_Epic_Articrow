using UnityEngine;

namespace YSJ
{
    public class PlayerHand : MonoBehaviour
    {
        void Update()
        {
            RotationHand();
        }

        void RotationHand()
        {
            Vector3 MousePosition = Managers.Input.MouseWorldPos;
            Vector2 Direction = (MousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}