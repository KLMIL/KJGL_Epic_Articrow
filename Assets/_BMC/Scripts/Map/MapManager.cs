using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSJ;
using static Define;

namespace BMC
{
    public class MapManager : MonoBehaviour
    {
        static MapManager s_instance;
        public static MapManager Instance => s_instance;

        [Header("전체 맵")]
        [field: SerializeField] public Room[] FullMap { get; private set; }
        public int MaxRow { get; private set; } = 6;
        public int MaxCol { get; private set; } = 5;

        [Header("방 생성 관련")]
        public Dictionary<RoomType, List<Room>> RoomTypeRoomListDict { get; private set; } = new Dictionary<RoomType, List<Room>>(); // 방 타입별 종류 (추후에 스테이지별로 관리할 수 있도록 수정 예정)
        Vector2 _roomOffset = new Vector2(14f, 26f); // row, col 간격

        [Header("방 이동 관련")]
        [field: SerializeField] public Room CurrentRoom { get; set; }
        [field: SerializeField] public Door CurrentDoor { get; set; }
        CameraController _cameraController;

        [Header("방 보상 관련")]
        public Dictionary<RoomType, List<GameObject>> RoomTypeRewardListDict { get; private set; } = new Dictionary<RoomType, List<GameObject>>(); // 방 타입별 보상 (추후에 스테이지별로 관리할 수 있도록 수정 예정)

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
            foreach (RoomType roomType in System.Enum.GetValues(typeof(RoomType)))
            {
                // 방 타입별 방 오브젝트
                Room[] rooms = Managers.Resource.LoadAll<Room>($"Prefabs/RoomTemplate/{roomType}");
                RoomTypeRoomListDict.Add(roomType, rooms.ToList());

                // 방 타입별 보상 오브젝트
                GameObject[] rewards = Managers.Resource.LoadAll<GameObject>($"Prefabs/Rewards/{roomType}");
                RoomTypeRewardListDict.Add(roomType, rewards.ToList());
            }

            FullMap = new Room[MaxRow * MaxCol];

            CreateRoomRandomInTypeAtPoint(RoomType.BossRoom);          // 보스 방 생성
            CreateRoomRandomInTypeAtPoint(RoomType.StartRoom);         // 시작 방 생성

            _cameraController = Camera.main.transform.root.GetComponent<CameraController>();
        }

        // 인덱스에 해당하는 방 반환
        public Room GetRoom(int row, int col)
        {
            if (row < 0 || row >= MaxRow || col < 0 || col >= MaxCol)
            {
                return null;
            }
            int idx = row * MaxCol + col;
            Room room = FullMap[idx];
            return room;
        }

        // 특정 지점에 특정 방 리스트 중 랜덤하게 생성
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
            if (row < 0 || row >= MaxRow || col < 0 || col >= MaxCol || idx < 0 || idx >= FullMap.Length)
            {
                Debug.LogError($"유효하지 않은 행/열: row({row}) col({col}). MaxRow: {MaxRow}, MaxCol: {MaxCol}");
                return null; // 유효하지 않은 경우 null 반환
            }

            // 해당 위치에 방이 없는 경우
            if (FullMap[idx] == null)
            {
                Room newRoom = CreateRoomRandomInType(roomType);
                if (newRoom != null)
                {
                    newRoom.transform.SetParent(transform);
                    newRoom.Init(row, col);
                    Vector2 spawnPos = new Vector2(col * _roomOffset.y, -row * _roomOffset.x);
                    newRoom.transform.position = spawnPos;
                    FullMap[idx] = newRoom;
                    CurrentRoom = newRoom;
                }
            }

            return FullMap[idx];
        }

        // 특정 지점에 특정 방 리스트 중 랜덤하게 생성
        public Room CreateRoomRandomInType(RoomType roomType)
        {
            Room newRoomPrefab = null;
            if(RoomTypeRoomListDict.TryGetValue(roomType, out var roomList))
            {
                newRoomPrefab = roomList[Random.Range(0, roomList.Count)];
            }
            return (newRoomPrefab != null) ? Instantiate(newRoomPrefab) : null;
        }
    }
}