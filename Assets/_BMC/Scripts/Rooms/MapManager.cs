using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager s_instance;
    public static MapManager Instance => s_instance;

    Room[,] _fullMap;
    public int Row { get; private set; } = 6;
    public int Col { get; private set; } = 5;

    [SerializeField] Room _currentRoom;
    [SerializeField] Vector2 _currentRoomPos; // 현재 방 위치(row, col)

    [Header("방 생성 간격")]
    [SerializeField] Vector2 _roomOffset = new Vector2(14f, 25f); // row, col 간격
    
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

    public void Init()
    {
        _fullMap = new Room[Row, Col];
        CreateRoom(RoomType.StartRoom); // 시작 방 생성
    }

    // 특정 방 생성
    public Room CreateRoom(RoomType roomType, int row = -1, int col = -1)
    {
        // 시작 방
        if(roomType == RoomType.StartRoom)
        {
            row = Row - 1;
            col = Random.Range(0, Col);
            _currentRoomPos = new Vector2(row, col);
        }

        Room newRoom = Managers.Resource.Load<Room>($"Prefabs/RoomTemplate/{roomType}");
        Debug.Log($"{roomType} 생성");
        if (newRoom != null)
        {
            Vector2 spawnPos = new Vector2(col * _roomOffset.y, -row * _roomOffset.x);
            Debug.LogWarning(spawnPos + "에 방이 소환됨");
            _currentRoom = Instantiate(newRoom, spawnPos, Quaternion.identity);
            _currentRoom.Init(row, col); // 방 초기화
        }

        return _currentRoom;

        //Room roomInstance = Instantiate(room, new Vector2(col, row), Quaternion.identity);
        //roomInstance.Init(row, col, roomType); // 방 초기화
    }

    // 현재 방의 위치에서 특정 방향으로 문이 유효한지 검사 (맵 끝 방인 경우, 해당 방향이 유효하지 않으면 방을 생성하지 말아야 함)
    // 0: 위, 1: 아래, 2: 왼쪽, 3: 오른쪽
    //public bool IsValidDoor(int direction)
    //{

    //}
}