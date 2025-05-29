using UnityEngine;

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
                Row = row,
                Col = col,
                IsVisited = true,
                IsCleared = true
            };

            gameObject.name = gameObject.name + $"({RoomData.Row}, {RoomData.Col})";
            DisposeInvalidDoor();
            OpenAllValidDoor();
        }
    }
}