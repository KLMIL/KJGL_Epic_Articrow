using System;
using static Define;

namespace BMC
{
    // 방 데이터
    [Serializable]
    public struct RoomData
    {
        public RoomType RoomType;   // 방 종류
        public RoomState RoomState; // 방 상태
        public int Row;             // 행
        public int Col;             // 열
        public bool IsVisited;      // 방문 여부
        public bool IsCleared;      // 클리어 여부
    }
}