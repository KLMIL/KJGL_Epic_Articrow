using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BMC
{
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
        float _openTime = 2.5f; // 문 열리는 시간

        [Header("클리어 관련")]
        EnemySpawner _enemySpawner;

        void Awake()
        {
            FindDoor();
        }

        void Update()
        {
            // (임시 코드) -> 나중에 Action으로 변경 필요
            if(_roomData.RoomType != RoomType.StartRoom && 
                _roomData.RoomType != RoomType.BossRoom)
            {
                _enemySpawner.IsClear();
            }
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

        #region 클리어 관련
        // 보상 소환
        public void SpawnReward()
        {
            if (_roomData.IsCleared && _roomData.RoomType != RoomType.BossRoom)
            {
                List<GameObject> rewardList = MapManager.Instance.RoomTypeRewardListDict[RoomData.RoomType];
                GameObject rewardObject = rewardList[Random.Range(0, rewardList.Count)];
                Instantiate(rewardObject, transform.position, Quaternion.identity);
            }
        }
        
        // 방 클리어 완료
        public void RoomClearComplete()
        {
            if (!_roomData.IsCleared)
            {
                Debug.LogError(" 방 클리어");
                _roomData.IsCleared = true;
                SpawnReward();
                GameManager.Instance.CameraController.SetCameraTargetRoom(transform);
                Invoke(nameof(OpenAllValidDoor), _openTime);
            }
        }
        #endregion

        #region 방 On/Off 관련

        public void DeactivateRoom()
        {
            _roomData.RoomState = RoomState.Deactivate;
            UI_InGameEventBus.OnDeactivateMinimapRoom?.Invoke(RoomData.Row * MapManager.Instance.MaxCol + RoomData.Col);
        }

        public void ActivateRoom()
        {
            _roomData.RoomState = RoomState.Active;
            UI_InGameEventBus.OnActiveMinimapRoom?.Invoke(RoomData.Row * MapManager.Instance.MaxCol + RoomData.Col);
        }

        // 적 소환
        public void SpawnEnemy()
        {
            if (!_roomData.IsCleared)
            {
                if (_roomData.RoomType != RoomType.StartRoom)
                {
                    _enemySpawner = GameManager.Instance.EnemySpawner;
                    _enemySpawner.Init();
                    if (_roomData.RoomType == RoomType.BossRoom)
                        _enemySpawner.SpawnBoss();
                    else
                        _enemySpawner.SpawnEnemy();
                }
            }
        }
        #endregion
    }
}