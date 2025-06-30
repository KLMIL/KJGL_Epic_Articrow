using UnityEngine;
using YSJ;
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

        protected StartPosition _startPosition; // 방 진입 시 시작 위치 

        [Header("클리어 관련")]
        protected EnemySpawner _enemySpawner;

        void Awake()
        {
            FindDoor();
            _startPosition = GetComponentInChildren<StartPosition>();
        }

        void Update()
        {
            //// (임시 코드) -> 나중에 Action으로 변경 필요
            //if (_roomData.RoomType != RoomType.StartRoom &&
            //    _roomData.RoomType != RoomType.BossRoom)
            //{
            //    _enemySpawner.IsClear();
            //}
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

        public void PlacePlayer()
        {
            PlayerManager.Instance.transform.position = _startPosition.transform.position; // 플레이어 위치 초기화
        }

        #region 클리어 관련
        // 보상 소환
        public void SpawnScarecrow()
        {
            if (_roomData.IsCleared)
            {
                GameObject scarecrowPrefab = Managers.Resource.Instantiate("ScarecrowPrefab");
                scarecrowPrefab.name = "Scarecrow";
                //Debug.LogError("허수아비 소환");
            }
            else
            {
                Debug.LogError("허수아비 소환 실패");
            }
        }
        
        // 방 클리어 완료
        public void RoomClearComplete()
        {
            if (!_roomData.IsCleared)
            {
                //Debug.LogError(" 방 클리어");
                _roomData.IsCleared = true;
                SpawnScarecrow();
                //GameManager.Instance.CameraController.SetCameraTargetRoom(transform);
                Invoke(nameof(OpenAllValidDoor), _openTime);
            }
        }
        #endregion
    }
}