using UnityEngine;
using static Define;

namespace BMC
{
    public class MagicRoom : Room
    {
        public override void Init(int row, int col)
        {
            // 방 데이터 초기화
            _roomData = new RoomData
            {
                RoomType = RoomType.MagicRoom,
                RoomState = RoomState.Undiscover,
                Row = row,
                Col = col,
                IsVisited = true,
                IsCleared = false,
            };

            gameObject.name = gameObject.name + $"({RoomData.Row}, {RoomData.Col})";
            DisposeInvalidDoor();
            //SpawnMonster();
            //OpenAllValidDoor();
        }
    }
}