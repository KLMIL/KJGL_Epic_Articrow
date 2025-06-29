using UnityEngine;

namespace BMC
{
    /// <summary>
    /// 플레이어가 문에 닿았는지 감지하고, 감지되면 다음 방으로 이동시키는 클래스
    /// </summary>
    public class DoorDetectionPlayer : MonoBehaviour
    {
        Door _door;

        public void Init(Door door)
        {
            _door = door;
        }

        #region OnTrigger
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _door.IsOpen)
            {
                Debug.Log("문 닿음");
                _door.NextStage();
            }
        }
        #endregion
    }
}