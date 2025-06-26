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
        float _openTime = 2.5f; // 문 열리는 시간

        [Header("클리어 관련")]
        EnemySpawner _enemySpawner;

        void Awake()
        {
            FindDoor();
        }

        public virtual void Init() { }

        #region 문 관련
        // 방에 있는 문 찾기
        void FindDoor()
        {
            _doors = GetComponentsInChildren<Door>();
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
        #endregion

        #region 클리어 관련
        // 보상 소환
        public void SpawnReward()
        {
            if (_roomData.IsCleared && _roomData.RoomType != RoomType.BossRoom)
            {
                List<GameObject> rewardList = StageManager.Instance.RoomTypeRewardListDict[RoomData.RoomType];
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
                //SpawnReward();
                //GameManager.Instance.CameraController.SetCameraTargetRoom(transform);
                Invoke(nameof(OpenAllValidDoor), _openTime);
            }
        }
        #endregion

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
    }
}