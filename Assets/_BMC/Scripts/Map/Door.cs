using System;
using System.Collections;
using UnityEngine;
using static Define;

namespace BMC
{
    public class Door : MonoBehaviour
    {
        [Header("기본 컴포넌트")]
        Animator _anim;
        SpriteRenderer _spriteRenderer;
        BoxCollider2D _boxCollider2D;

        [Header("상태")]
        [SerializeField] Room _currentRoom;
        [SerializeField] bool _isOpen = false;

        [Header("다음 방 관련")]
        [field: SerializeField] public DoorPosition DoorPosition { get; private set; }    // 문 위치
        [SerializeField] RoomType _willCreateRoomType;
        [field: SerializeField] public Room NextRoom { get; set; }
        float _doorSpawnPlayerPositionOffset = 0.7f;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _currentRoom = transform.root.GetComponent<Room>();
        }

        #region 문 개방/폐쇄
        // 열기
        public void Open()
        {
            _isOpen = true;
            _boxCollider2D.isTrigger = true;
            _anim.Play("Open");
        }

        // 닫기
        public void Close()
        {
            _isOpen = false;
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
            // 1. 현재 방 비활성화
            MapManager.Instance.CurrentRoom.DeactivateRoom();

            // 2. 현재 방의 선택된 문으로 설정
            _currentRoom.SelectedDoor = this;

            // 3. 다음 방의 정보 가져오기
            Tuple<int, int> nextRoomIndex = GetNextRoomIndex();
            NextRoom = MapManager.Instance.GetRoom(nextRoomIndex.Item1, nextRoomIndex.Item2);

            // 4. 클리어 했는데 다음 방이 없다면, 방 생성 UI 띄우기
            if (_currentRoom.RoomData.IsCleared && NextRoom == null)
            {
                // 방 선택 UI 띄우고, 문 정보 넘기기
                UI_InGameEventBus.OnToggleChoiceRoomCanvas?.Invoke();
                MapManager.Instance.CurrentDoor = this;
                return;
            }

            TransferToNextRoom(playerTransform);
        }
        
        // 다음 방으로 이동
        public void TransferToNextRoom(Transform playerTransform)
        {
            if (NextRoom != null)
            {
                // TODO: 아이작처럼 하려면 벽 테두리에 간격이 있어야 일정한 소환 위치가 나올 것 같음, 이를 아트에 반영하고 코드 수정해야함
                // 다음 방으로 입장할 때의 입장하는 문 위치에서 일정 거리만큼 떨어져서 이동
                MapManager.Instance.CurrentRoom = NextRoom;
                MapManager.Instance.CurrentRoom.ActivateRoom();
                DoorPosition nextRoomDoorPosition = GetDoorPositionOfNextRoom();
                Door spawnDoor = NextRoom.GetDoor(nextRoomDoorPosition);
                _doorSpawnPlayerPositionOffset = (nextRoomDoorPosition == DoorPosition.Up || nextRoomDoorPosition == DoorPosition.Down) ? 1.5f : 1f;
                playerTransform.position = spawnDoor.transform.position + spawnDoor.transform.up * _doorSpawnPlayerPositionOffset;
                GameManager.Instance.CameraController.SetCameraTargetRoom(NextRoom.transform);  // 카메라 전환

                // 카메라 전환 (임시 코드)
                if (NextRoom.RoomData.RoomType != RoomType.StartRoom && NextRoom.RoomData.RoomType != RoomType.BossRoom && !NextRoom.RoomData.IsCleared)
                {
                    GameManager.Instance.CameraController.SetCameraTargetPlayer(PlayerManager.Instance.transform);
                    //CameraController.Instance.SetCameraTargetPlayer(PlayerManager.Instance.transform); // 카메라 전투 모드
                }

                MapManager.Instance.CurrentRoom.SpawnEnemy(); // 적 소환

                if (!NextRoom.RoomData.IsCleared)
                    NextRoom.CloseAllValidDoor();
            }

            _currentRoom.SelectedDoor = null;
        }

        // 다음 방의 행,열 반환
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

        // 문 열릴 때 문 파괴되는 애니메이션 이벤트
        public void OnOpenAnimationEvent()
        {
            StartCoroutine(DestroyAfterOpenCoroutine());
        }

        IEnumerator DestroyAfterOpenCoroutine()
        {
            float alphaValue = 1;

            Color color = _spriteRenderer.color;
            while (alphaValue > 0)
            {
                alphaValue -= Time.deltaTime;
                color.a = alphaValue;
                _spriteRenderer.color = color;
                yield return null;
            }

            alphaValue = 0;
            color.a = alphaValue;
            _spriteRenderer.color = color;
        }

        #region OnTrigger
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _isOpen)
            {
                if (collision.isTrigger) // 플레이어 Hit box 무시
                    return;
                TryTransferToNextRoom(collision.transform);
            }
        }
        #endregion
    }
}