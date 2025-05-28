using UnityEngine;

public class StartRoom : Room
{
    Door[] doors;

    void Awake()
    {
        doors = GetComponentsInChildren<Door>();
    }

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

        // 모든 문 개방
        foreach (var door in doors)
        {
            door.Open();
        }
    }
}