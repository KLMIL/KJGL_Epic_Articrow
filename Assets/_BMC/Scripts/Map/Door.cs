using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BMC
{
    public enum DoorPosition
    {
        None = 0,
        Up = 1,
        Left = 2,
        Right = 3,
        Down = 4,
    }

    public class Door : MonoBehaviour
    {
        [Header("기본 컴포넌트")]
        SpriteRenderer _spriteRenderer;
        BoxCollider2D _boxCollider2D;

        [Header("상태")]
        [SerializeField] Room _currentRoom;
        [SerializeField] bool _isValid = true;
        [SerializeField] bool _isOpen = false;
        Color _openColor = Color.yellow;
        Color _closedColor = Color.black;

        [Header("다음 방 관련")]
        [field: SerializeField] public DoorPosition DoorDirection { get; private set; }    // 문 위치
        [SerializeField] RoomType _willCreateRoomType;
        [SerializeField] Room _nextRoom;
        public static event Action<Transform> OnEnterAction;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _currentRoom = transform.root.GetComponent<Room>();
        }

        #region 문 개방/폐쇄
        // 열기
        public void Open()
        {
            _isOpen = true;
            _spriteRenderer.color = _openColor;
            _boxCollider2D.isTrigger = true;
        }

        // 닫기
        public void Close()
        {
            _isOpen = false;
            _spriteRenderer.color = _closedColor;
            _boxCollider2D.isTrigger = false;
        }

        // 폐기(아예 벽으로 만들고 문 기능 못하게 하기)
        public void Dispose()
        {
            Close();
            enabled = false;
        }
        #endregion

        // 다음 방의 문 방향 반환
        public DoorPosition GetDoorPositionOfNextRoom()
        {
            int doorPositionTypeCount = Enum.GetNames(typeof(DoorPosition)).Length;
            int position = doorPositionTypeCount - (int)DoorDirection;
            return Util.IntToEnum<DoorPosition>(position);
        }

        // 입장
        public void Enter(Transform playerTransform)
        {
            Tuple<int, int> nextRoomIndex = GetNextRoomIndex();
            _nextRoom = MapManager.Instance.GetRoom(nextRoomIndex.Item1, nextRoomIndex.Item2);

            // 현재 방을 클리어했고, 아직 다음 방이 생성되지 않은 경우
            if (_currentRoom.RoomData.IsCleared && _nextRoom == null)
            {
                // TODO 추후에 선택해서 방 만들 수 있도록 하기
                // 방 랜덤 생성
                if (_willCreateRoomType == RoomType.None)
                {
                    int randomRoomType = Random.Range(3, Enum.GetNames(typeof(RoomType)).Length);
                    _willCreateRoomType = Util.IntToEnum<RoomType>(randomRoomType);
                }
                Room newRoom = MapManager.Instance.CreateRoom(_willCreateRoomType, nextRoomIndex.Item1, nextRoomIndex.Item2);
                _nextRoom = newRoom;
            }

            if (_nextRoom != null)
            {
                // 다음 방으로 입장할 때의 입장하는 문 위치에서 1만큼 떨어져서 이동
                MapManager.Instance.CurrentRoom = _nextRoom;
                DoorPosition nextRoomDoorPosition = GetDoorPositionOfNextRoom();
                Door spawnDoor = _nextRoom.GetDoor(nextRoomDoorPosition);
                playerTransform.position = spawnDoor.transform.position + spawnDoor.transform.up;
                Debug.LogWarning($"다음 방에서 소환될 위치: {spawnDoor.transform.position + spawnDoor.transform.up}");
                OnEnterAction.Invoke(_nextRoom.transform);
                _nextRoom.CloseAllValidDoor();
            }
        }

        public Tuple<int,int> GetNextRoomIndex()
        {
            // 다음 방 위치 계산
            int row = _currentRoom.RoomData.Row, col = _currentRoom.RoomData.Col;
            switch (DoorDirection)
            {
                case DoorPosition.Up:
                    row -= 1;
                    //Debug.Log("다음 방의 아래에서 등장");
                    break;
                case DoorPosition.Down:
                    row += 1;
                    //Debug.Log("다음 방의 위에서 등장");
                    break;
                case DoorPosition.Left:
                    col -= 1;
                    //Debug.Log("다음 방의 오른쪽에서 등장");
                    break;
                case DoorPosition.Right:
                    col += 1;
                    //Debug.Log("다음 방의 왼쪽에서 등장");
                    break;
                default:
                    //Debug.Log("???");
                    break;
            }
            Tuple<int, int> nextRoomIndex = new Tuple<int, int>(row, col);
            return nextRoomIndex;
        }

        #region OnTrigger
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _isOpen)
            {
                //Debug.Log("문 진입");
                Enter(collision.transform);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _isOpen)
            {
                //Debug.Log("문 나감");
            }
        }
        #endregion
    }
}