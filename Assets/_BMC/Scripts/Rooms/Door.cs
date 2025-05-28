using NUnit.Framework.Constraints;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DoorPosition
{
    None = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4
}

public class Door : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    Color _openColor = Color.yellow;
    Color _closedColor = Color.black;

    [field: SerializeField] public DoorPosition DoorDirection { get; private set; }    // 문 위치

    [SerializeField] Room _currentRoom;

    bool _isCreatedNextRoom = false; // 다음 방이 생성되었는지 여부

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _currentRoom = transform.root.GetComponent<Room>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Open();
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Close();
        }
    }

    // 열기
    public void Open()
    {
        _spriteRenderer.color = _openColor;
        _boxCollider2D.isTrigger = true;
    }

    // 닫기
    public void Close()
    {
        _spriteRenderer.color = _closedColor;
        _boxCollider2D.isTrigger = false;
    }

    // 입장
    public void Enter()
    {
        int row = _currentRoom.RoomData.Row, col = _currentRoom.RoomData.Col;
        switch (DoorDirection)
        {
            case DoorPosition.Up:
                row -= 1;
                break;
            case DoorPosition.Down:
                row += 1;
                Debug.Log("다음 방의 문은 아래쪽입니다.");
                break;
            case DoorPosition.Left:
                col -= 1;
                Debug.Log("다음 방의 문은 왼쪽입니다.");
                break;
            case DoorPosition.Right:
                col += 1;
                Debug.Log("다음 방의 문은 오른쪽입니다.");
                break;
            default:
                Debug.Log("다음 방의 문 위치가 정의되지 않았습니다.");
                break;
        }

        if (_currentRoom.RoomData.IsCleared && !_isCreatedNextRoom)
        {
            // 방 랜덤 생성, 추후에 선택해서 방 만들 수 있도록 만들기
            int randomRoomType = Random.Range(2, Enum.GetNames(typeof(RoomType)).Length);
            RoomType roomType = Util.IntToEnum<RoomType>(randomRoomType);
            Room newRoom = MapManager.Instance.CreateRoom(roomType, row, col);
            _isCreatedNextRoom = true;
        }

        // 다음 방으로 입장할 때의 문 위치
        DoorPosition nextRoomDoorPosition = GetDoorPositionOfNextRoom();
        
    }

    // 다음 방의 문 방향 반환
    public DoorPosition GetDoorPositionOfNextRoom()
    {
        int doorPositionTypeCount = Enum.GetNames(typeof(DoorPosition)).Length;
        int position = doorPositionTypeCount - (int)DoorDirection;
        return Util.IntToEnum<DoorPosition>(position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("문 진입");
            Enter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("문 나감");
        }
    }
}