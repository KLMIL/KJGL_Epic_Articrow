using System;
using static Define;

namespace BMC
{
    // 방 데이터
    [Serializable]
    public struct RoomData
    {
        public RoomType RoomType;   // 방 종류
        public bool IsCleared;      // 클리어 여부
    }
}