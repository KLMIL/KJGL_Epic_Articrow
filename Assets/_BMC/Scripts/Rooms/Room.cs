using System;
using UnityEngine;

// 방 종류
public enum RoomType
{
    None,
    StartRoom,      // 시작
    BossRoom,       // 보스
    MagicRoom,      // 마법
    ArtifactRoom,   // 아티팩트
    HealRoom,       // 회복
}

// 방 데이터
[Serializable]
public struct RoomData
{
    public RoomType RoomType;   // 방 종류
    public int Row;             // 행
    public int Col;             // 열
    public bool IsVisited;      // 방문 여부
    public bool IsCleared;      // 클리어 여부

    public RoomData(RoomType roomType, int row, int col, bool isVisited, bool isCleared)
    {
        RoomType = roomType;
        Row = row;
        Col = col;
        IsVisited = isVisited;
        IsCleared = isCleared;
    }
}

public class Room : MonoBehaviour
{
    protected RoomData _roomData;
    public RoomData RoomData => _roomData;

    public virtual void Init(int row, int col)
    {
        _roomData = new RoomData
        {
            RoomType = RoomType.None,
            Row = row,
            Col = col,
            IsVisited = false,
            IsCleared = false
        };
    }
}