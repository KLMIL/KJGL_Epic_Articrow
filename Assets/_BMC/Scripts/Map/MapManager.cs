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

        [Header("방 생성 관련")]
        [SerializeField] Vector2 _roomOffset = new Vector2(14f, 25f); // row, col 간격

        // Vertical Slice를 위한 방 생성 부분 /
        Room[] _testRooms;

        /* ------------------------------- */

        [Header("방 이동 관련")]
        [field: SerializeField] public Room CurrentRoom { get; set; }
        [field: SerializeField] public Door CurrentDoor { get; set; }
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

            CreateRoomAtPoint(RoomType.BossRoom);          // 보스 방 생성
            CreateRoomAtPoint(RoomType.StartRoom);         // 시작 방 생성

            _cameraController = Camera.main.transform.root.GetComponent<CameraController>();
        }

        // 특정 지점에 방 생성
        public Room CreateRoomAtPoint(RoomType roomType, int row = -1, int col = -1)
        {
            // 시작 방 및 보스 방
            if (roomType == RoomType.StartRoom || roomType == RoomType.BossRoom)
            {
                row = (roomType == RoomType.StartRoom) ? MaxRow - 1 : 0;
                col = Random.Range(0, MaxCol);
            }

            // 유효한 인덱스인지 확인
            int idx = row * MaxCol + col;
            if (row < 0 || row >= MaxRow || col < 0 || col >= MaxCol || idx < 0 || idx >= _fullMap.Length)
            {
                Debug.LogError($"유효하지 않은 행/열: row({row}) col({col}). MaxRow: {MaxRow}, MaxCol: {MaxCol}");
                return null; // 유효하지 않은 경우 null 반환
            }

            // 해당 위치에 방이 없는 경우
            if (_fullMap[idx] == null)
            {
                Room newRoom = CreateRoom(roomType);
                if (newRoom != null)
                {
                    newRoom.transform.SetParent(transform);
                    newRoom.Init(row, col);
                    Vector2 spawnPos = new Vector2(col * _roomOffset.y, -row * _roomOffset.x);
                    newRoom.transform.position = spawnPos;
                    _fullMap[idx] = newRoom;
                    CurrentRoom = newRoom;
                }
            }
            return _fullMap[idx];
        }

        // 방 생성
        public Room CreateRoom(RoomType roomType)
        {
            // TODO: 방 타입별로 폴더를 만들어서 로드하도록 방을 해야함 (현재 MagicRoom만 폴더로 하게 해놓음)
            Room newRoomPrefab = YSJ.Managers.Resource.Load<Room>($"Prefabs/RoomTemplate/{roomType}");
            return (newRoomPrefab != null) ? Instantiate(newRoomPrefab) : null;
        }

        // 인덱스에 해당하는 방 반환
        public Room GetRoom(int row, int col)
        {
            if (row < 0 || row >= MaxRow || col < 0 || col >= MaxCol)
            {
                return null;
            }
            int idx = row * MaxCol + col;
            Room room = _fullMap[idx];
            return room;
        }

        // TODO: Vertical Slide 후 삭제
        // 특정 지점에 방 생성
        public Room CreateRoomRandomInTypeAtPoint(RoomType roomType, int row = -1, int col = -1)
        {
            // 시작 방 및 보스 방
            if (roomType == RoomType.StartRoom || roomType == RoomType.BossRoom)
            {
                row = (roomType == RoomType.StartRoom) ? MaxRow - 1 : 0;
                col = Random.Range(0, MaxCol);
            }

            // 유효한 인덱스인지 확인
            int idx = row * MaxCol + col;
            if (row < 0 || row >= MaxRow || col < 0 || col >= MaxCol || idx < 0 || idx >= _fullMap.Length)
            {
                Debug.LogError($"유효하지 않은 행/열: row({row}) col({col}). MaxRow: {MaxRow}, MaxCol: {MaxCol}");
                return null; // 유효하지 않은 경우 null 반환
            }

            // 해당 위치에 방이 없는 경우
            if (_fullMap[idx] == null)
            {
                Room newRoom = CreateRoomRandomInType(roomType);
                if (newRoom != null)
                {
                    newRoom.transform.SetParent(transform);
                    newRoom.Init(row, col);
                    Vector2 spawnPos = new Vector2(col * _roomOffset.y, -row * _roomOffset.x);
                    newRoom.transform.position = spawnPos;
                    _fullMap[idx] = newRoom;
                    CurrentRoom = newRoom;
                }
            }
            return _fullMap[idx];
        }

        public Room CreateRoomRandomInType(RoomType roomType)
        {
            if(_testRooms == null)
            {
                _testRooms = YSJ.Managers.Resource.LoadAll<Room>($"Prefabs/RoomTemplate/{roomType}");
            }
            Room newRoomPrefab = _testRooms[Random.Range(0, _testRooms.Length)];
            return (newRoomPrefab != null) ? Instantiate(newRoomPrefab) : null;
        }
    }
}