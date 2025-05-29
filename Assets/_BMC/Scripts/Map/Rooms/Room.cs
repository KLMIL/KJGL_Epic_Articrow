using System.Collections.Generic;
using UnityEngine;

namespace BMC
{
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

    // 방
    public class Room : MonoBehaviour
    {
        [SerializeField] protected RoomData _roomData;                       // 방 데이터
        [SerializeField] protected Door[] _doors;                            // 방에 있는 문들
        [SerializeField] protected Dictionary<DoorPosition, Door> _doorDict; // 방에 있는 문들

        public RoomData RoomData => _roomData;
        public Door[] Doors => _doors;

        int[] directionY = { -1, 0, 0, 1 }; // 상, 좌, 우, 하
        int[] directionX = { 0, -1, 1, 0 }; // 상, 좌, 우, 하

        void Awake()
        {
            FindDoor();
            RearrangeDoor();
        }

        public virtual void Init(int row, int col)
        {
            _roomData = new RoomData
            {
                RoomType = RoomType.None,
                Row = row,
                Col = col,
                IsVisited = false,
                IsCleared = false,
            };
        }

        // 문 리스트 정렬
        void RearrangeDoor()
        {
            for (int i = 0; i < _doors.Length; i++)
            {
                int idx = (int)_doors[i].DoorDirection - 1;
                (_doors[i], _doors[idx]) = (_doors[idx], _doors[i]);
            }
        }

        void FindDoor()
        {
            _doors = GetComponentsInChildren<Door>();
            _doorDict = new Dictionary<DoorPosition, Door>();
            foreach (var door in _doors)
            {
                _doorDict.Add(door.DoorDirection, door);
            }
        }

        #region 문 개방/폐쇄
        // 유효한 모든 문 열기
        public void OpenAllValidDoor()
        {
            foreach (var door in _doors)
            {
                if (door.enabled)
                    door.Open();
            }
        }

        // 유효한 모든 문 닫기
        public void CloseAllValidDoor()
        {
            foreach (var door in _doors)
            {
                if (door.enabled)
                    door.Close();
            }
        }

        // 문 폐기
        public void DisposeInvalidDoor()
        {
            int row = _roomData.Row, col = _roomData.Col;
            int newRow = -1, newCol = -1;
            for (int i=0; i<4; i++)
            {
                newRow = row + directionY[i];
                newCol = col + directionX[i];

                if(newRow < 0 || newRow >= MapManager.Instance.MaxRow ||
                   newCol < 0 || newCol >= MapManager.Instance.MaxCol)
                {
                    // 맵 밖으로 나가는 문은 폐기
                    DoorPosition doorPosition = Util.IntToEnum<DoorPosition>(i + 1);
                    Door door = GetDoor(doorPosition);
                    if (door != null)
                    {
                        door.Dispose();
                        _doorDict.Remove(doorPosition);
                    }
                }
            }
        }
        #endregion

        // 특정 방향의 문 반환
        public Door GetDoor(DoorPosition doorPosition)
        {
            Door targetDoor = null;
            _doorDict.TryGetValue(doorPosition, out targetDoor);
            return targetDoor;
        }

        // 방 클리어 완료
        public void Complete()
        {
            _roomData.IsCleared = true;
            OpenAllValidDoor();
        }
    }
}