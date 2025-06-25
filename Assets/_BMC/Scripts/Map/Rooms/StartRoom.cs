using UnityEngine;
using static Define;

namespace BMC
{
    public class StartRoom : Room
    {
        public override void Init()
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.StartRoom,
                RoomState = RoomState.Active,
                IsCleared = true
            };

            OpenAllValidDoor();
        }

        void Start()
        {
            PlacePlayer();
            Init();
        }

        public void PlacePlayer()
        {
            PlayerManager.Instance.transform.position = transform.position; // 플레이어 위치 초기화
        }
    }
}