using System.Collections.Generic;
using UnityEngine;
using YSJ;

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
        [Header("방 정보")]
        [SerializeField] protected RoomData _roomData;                       // 방 데이터
        public RoomData RoomData => _roomData;

        [Header("문 관련")]
        [SerializeField] protected Door[] _doors;
        [SerializeField] protected Dictionary<DoorPosition, Door> _doorDict; // 문 딕셔너리 (문 위치, 문)
        [field: SerializeField] public Door SelectedDoor { get; set; } // 선택된 문
        int[] _doorDirectionY = { -1, 0, 0, 1 };    // 상, 좌, 우, 하
        int[] _doorDirectionX = { 0, -1, 1, 0 };    // 상, 좌, 우, 하

        void Awake()
        {
            FindDoor();
            //RearrangeDoor();
        }

        public virtual void Init(int row, int col) { }

        #region 문 관련
        // 방에 있는 문 찾기
        void FindDoor()
        {
            _doors = GetComponentsInChildren<Door>();
            _doorDict = new Dictionary<DoorPosition, Door>();
            foreach (var door in _doors)
            {
                _doorDict.Add(door.DoorPosition, door);
                //Debug.Log(door.name);
            }
        }

        // 특정 방향의 문 반환
        public Door GetDoor(DoorPosition doorPosition)
        {
            Door targetDoor = _doorDict.TryGetValue(doorPosition, out targetDoor) ? targetDoor : null;
            return targetDoor;
        }

        // 문 리스트 정렬
        void RearrangeDoor()
        {
            int idx = -1;
            for (int i = 0; i < _doors.Length; i++)
            {
                idx = (int)_doors[i].DoorPosition - 1;
                (_doors[i], _doors[idx]) = (_doors[idx], _doors[i]);
            }
        }

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
            for (int i = 0; i < 4; i++)
            {
                // 맵 밖으로 나갈 수 있는 문은 폐기
                newRow = row + _doorDirectionY[i];
                newCol = col + _doorDirectionX[i];
                if (newRow < 0 || newRow >= MapManager.Instance.MaxRow || newCol < 0 || newCol >= MapManager.Instance.MaxCol)
                {
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

        // 보상 소환 (임시)
        public void SpawnReward()
        {
            if (_roomData.IsCleared)
            {
                int index = Random.Range(0, 4);
                string path = $"Prefabs/Rewards/{_roomData.RoomType}/{index}";
                GameObject rewardObject = Managers.Resource.Load<GameObject>(path);
                if (rewardObject == null)
                {
                    Debug.Log("보상 오프젝트 없음");
                    return;
                }
                else
                {
                    GameObject reward = Instantiate(rewardObject, transform.position, Quaternion.identity);
                }
            }
        }

        // 방 클리어 완료
        public void Complete()
        {
            if (!_roomData.IsCleared)
            {
                _roomData.IsCleared = true;
                SpawnReward();
                OpenAllValidDoor();
            }
        }
    }
}