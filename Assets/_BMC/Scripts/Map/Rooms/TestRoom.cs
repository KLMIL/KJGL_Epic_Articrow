using UnityEngine;
using static Define;

namespace BMC
{
    public class TestRoom : Room
    {
        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.None,
                RoomState = RoomState.Undiscover,
                IsCleared = false,
            };
        }

        void Start()
        {
            StageManager.Instance.CurrentRoom = this; // 현재 방 설정
            PlacePlayer();
            Init();
        }

        public void PlacePlayer()
        {
            PlayerManager.Instance.transform.position = transform.position; // 플레이어 위치 초기화
        }
    }
}