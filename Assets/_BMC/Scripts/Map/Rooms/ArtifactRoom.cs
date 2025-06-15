using UnityEngine;
using static Define;

namespace BMC
{
    public class ArtifactRoom : Room
    {
        public override void Init(int row, int col)
        {
            _roomData = new RoomData
            {
                RoomType = RoomType.ArtifactRoom,
                RoomState = RoomState.Deactivate,
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