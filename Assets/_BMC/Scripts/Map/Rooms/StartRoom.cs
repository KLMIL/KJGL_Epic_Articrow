using UnityEngine;
using static Define;

namespace BMC
{
    public class StartRoom : Room
    {
        public override void Init(int row, int col)
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.StartRoom,
                RoomState = RoomState.Active,
                Row = row,
                Col = col,
                IsVisited = true,
                IsCleared = true
            };

            gameObject.name = gameObject.name + $"({RoomData.Row}, {RoomData.Col})";
            DisposeInvalidDoor();
            OpenAllValidDoor();
        }

        void Start()
        {
            PlacePlayer();
            UI_InGameEventBus.OnActiveMinimapRoom?.Invoke(RoomData.Row * MapManager.Instance.MaxCol + RoomData.Col);
        }

        public void PlacePlayer()
        {
            PlayerManager.Instance.transform.position = transform.position; // 플레이어 위치 초기화
        }
    }
}