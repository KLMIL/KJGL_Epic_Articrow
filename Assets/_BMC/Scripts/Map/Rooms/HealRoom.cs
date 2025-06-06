using UnityEngine;

namespace BMC
{
    public class HealRoom : Room
    {
        public override void Init(int row, int col)
        {
            _roomData = new RoomData
            {
                RoomType = RoomType.HealRoom,
                Row = row,
                Col = col,
                IsVisited = true,
                IsCleared = false,
            };

            gameObject.name = gameObject.name + $"({RoomData.Row}, {RoomData.Col})";
            DisposeInvalidDoor();
            SpawnMonster();
            //OpenAllValidDoor();
        }
    }
}