using BMC;
using UnityEngine;

namespace YSJ
{
    public class PlayerHand : MonoBehaviour
    {
        public bool CanHandling = true;

        public RightHand RightHand;

        void Awake()
        {
            RightHand = GetComponentInChildren<RightHand>();
        }

        void Update()
        {
            if (CanHandling && !PlayerManager.Instance.PlayerHurt.IsDead)
            {
                RotationHand();
            }
        }

        void RotationHand()
        {
            Vector3 mousePosition = Managers.Input.MouseWorldPos;
            Vector2 direction = (mousePosition - transform.position).normalized;
            //float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            //this.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            this.transform.up += (Vector3)direction * 60f * Time.deltaTime;
        }
    }
}