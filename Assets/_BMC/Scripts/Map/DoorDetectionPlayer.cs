using UnityEngine;
using static Define;

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

                if (collision.isTrigger) // 플레이어 Hit box 무시
                    return;

                // TODO: 임시 코드
                if (_door.CurrentRoom.RoomData.RoomType == RoomType.BossRoom && _door.DoorPosition == DoorPosition.Up)
                {
                    _door.NextStage();
                }
                else
                {
                    _door.TryTransferToNextRoom(collision.transform);
                }
            }
        }
        #endregion
    }
}