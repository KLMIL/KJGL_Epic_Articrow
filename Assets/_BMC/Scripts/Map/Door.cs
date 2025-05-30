using System;
using UnityEngine;

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
        [SerializeField] bool _isOpen = false;
        Color _openColor = Color.yellow;
        Color _closedColor = Color.black;

        [Header("다음 방 관련")]
        [field: SerializeField] public DoorPosition DoorPosition { get; private set; }    // 문 위치
        [SerializeField] RoomType _willCreateRoomType;
        [field: SerializeField] public Room NextRoom { get; set; }
        public static Action<Transform> OnTransferToNextRoom;
        float _doorSpawnPlayerPositionOffset = 0.7f;

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
            int position = doorPositionTypeCount - (int)DoorPosition;
            return Util.IntToEnum<DoorPosition>(position);
        }

        // 다음 방으로 이동 시도
        public void TryTransferToNextRoom(Transform playerTransform)
        {
            // 1. 현재 방의 선택된 문으로 설정
            _currentRoom.SelectedDoor = this;

            // 2. 다음 방의 정보 가져오기
            Tuple<int, int> nextRoomIndex = GetNextRoomIndex();
            NextRoom = MapManager.Instance.GetRoom(nextRoomIndex.Item1, nextRoomIndex.Item2);

            // 3. 클리어 했는데 다음 방이 없다면, 방 생성 UI 띄우기
            if (_currentRoom.RoomData.IsCleared && NextRoom == null)
            {
                // 방 선택 UI 띄우고, 문 정보 넘기기
                UI_EventBus.OnToggleChoiceRoomCanvas?.Invoke();
                MapManager.Instance.CurrentDoor = this;
                return;
            }

            TransferToNextRoom(playerTransform);
        }
        
        // 다음 방으로 이동
        public void TransferToNextRoom(Transform playerTransform)
        {
            if(NextRoom != null)
            {
                // TODO: 아이작처럼 하려면 벽 테두리에 간격이 있어야 일정한 소환 위치가 나올 것 같음, 이를 아트에 반영하고 코드 수정해야함
                // 다음 방으로 입장할 때의 입장하는 문 위치에서 일정 거리만큼 떨어져서 이동
                MapManager.Instance.CurrentRoom = NextRoom;
                DoorPosition nextRoomDoorPosition = GetDoorPositionOfNextRoom();
                Door spawnDoor = NextRoom.GetDoor(nextRoomDoorPosition);
                _doorSpawnPlayerPositionOffset = (nextRoomDoorPosition == DoorPosition.Up) ? 1.3f : 0.7f;
                playerTransform.position = spawnDoor.transform.position + spawnDoor.transform.up * _doorSpawnPlayerPositionOffset;
                OnTransferToNextRoom.Invoke(NextRoom.transform);

                if (!NextRoom.RoomData.IsCleared)
                    NextRoom.CloseAllValidDoor();
            }

            _currentRoom.SelectedDoor = null;
        }

        public Tuple<int, int> GetNextRoomIndex()
        {
            // 다음 방 위치 계산
            int row = _currentRoom.RoomData.Row, col = _currentRoom.RoomData.Col;
            switch (DoorPosition)
            {
                case DoorPosition.Up:
                    row -= 1;
                    break;
                case DoorPosition.Down:
                    row += 1;
                    break;
                case DoorPosition.Left:
                    col -= 1;
                    break;
                case DoorPosition.Right:
                    col += 1;
                    break;
                default:
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
                TryTransferToNextRoom(collision.transform);
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