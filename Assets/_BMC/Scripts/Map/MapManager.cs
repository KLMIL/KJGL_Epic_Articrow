using UnityEngine;

namespace BMC
{
    public class MapManager : MonoBehaviour
    {
        static MapManager s_instance;
        public static MapManager Instance => s_instance;

        [Header("전체 맵")]
        [SerializeField] Room[] _fullMap;
        public int MaxRow { get; private set; } = 6;
        public int MaxCol { get; private set; } = 5;

        [Header("방 생성 간격")]
        [SerializeField] Vector2 _roomOffset = new Vector2(14f, 25f); // row, col 간격

        [Header("방 이동 관련")]
        [field: SerializeField] public Room CurrentRoom { get; set; }
        CameraController _cameraController;

        void Awake()
        {
            if (Instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Init();
        }

        void Start()
        {
            _cameraController.SetCameraTarget(CurrentRoom.transform); // 현재 방 카메라 타겟 설정
        }

        public void Init()
        {
            _fullMap = new Room[MaxRow * MaxCol];
            CreateRoom(RoomType.BossRoom);          // 보스 방 생성
            CreateRoom(RoomType.StartRoom);         // 시작 방 생성

            _cameraController = Camera.main.transform.root.GetComponent<CameraController>();
        }

        // 특정 방 생성
        public Room CreateRoom(RoomType roomType, int row = -1, int col = -1)
        {
            // 시작 방 및 보스 방
            if (roomType == RoomType.StartRoom || roomType == RoomType.BossRoom)
            {
                row = (roomType == RoomType.StartRoom) ? MaxRow - 1 : 0;
                col = Random.Range(0, MaxCol);
            }

            int idx = row * MaxCol + col;

            // 유효한 인덱스인지 확인
            if(row < 0 || row >= MaxRow || col < 0 || col >= MaxCol || idx < 0 || idx >=_fullMap.Length)
            {
                Debug.LogError($"유효하지 않은 행/열: row({row}) col({col}). MaxRow: {MaxRow}, MaxCol: {MaxCol}");
                return null; // 유효하지 않은 경우 null 반환
            }

            // 해당 위치에 방이 없는 경우
            if (_fullMap[idx] == null)
            {
                Room newRoom = Managers.Resource.Load<Room>($"Prefabs/RoomTemplate/{roomType}");
                if (newRoom != null)
                {
                    Vector2 spawnPos = new Vector2(col * _roomOffset.y, -row * _roomOffset.x);
                    //Debug.LogWarning(spawnPos + "에 방이 소환됨");
                    Room newRoomInstance = Instantiate(newRoom, spawnPos, Quaternion.identity);
                    CurrentRoom = newRoomInstance;
                    newRoomInstance.Init(row, col); // 방 초기화
                    _fullMap[idx] = newRoomInstance; // 방을 맵에 추가
                }
            }
            return _fullMap[idx];
        }

        // 인덱스에 해당하는 방 반환
        public Room GetRoom(int row, int col)
        {
            if(row < 0 || row >= MaxRow || col < 0 || col >= MaxCol)
            {
                return null;
            }
            int idx = row * MaxCol + col;
            Room room = _fullMap[idx];
            return room;
        }
    }
}