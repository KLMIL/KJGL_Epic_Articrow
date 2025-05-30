using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace BMC
{
    public class RoomBtn : MonoBehaviour
    {
        [SerializeField] RoomType _roomType; // 방 타입
        Button _btn;
        TextMeshProUGUI _text;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();

            _text.text = _roomType.ToString();
            _btn.onClick.AddListener(() => OnClickedCreateRoom());
        }

        public void OnClickedCreateRoom()
        {
            Debug.Log($"{_roomType} 생성 버튼이 클릭되었습니다.");
            Room currentRoom = MapManager.Instance.CurrentRoom;
            Door currentSeletedDoor = currentRoom.SelectedDoor;
            Tuple<int, int> nextRoomIndex = currentSeletedDoor.GetNextRoomIndex();
            Room nextRoom = MapManager.Instance.GetRoom(nextRoomIndex.Item1, nextRoomIndex.Item2);

            if (currentRoom.RoomData.IsCleared && nextRoom == null)
            {
                Room newRoom = MapManager.Instance.CreateRoomAtPoint(_roomType, nextRoomIndex.Item1, nextRoomIndex.Item2);
                nextRoom = newRoom;
                currentSeletedDoor.NextRoom = nextRoom; // 선택된 문에 다음 방 설정
            }

            Transform playerTransform = FindAnyObjectByType<DummyPlayerController>().transform;
            currentSeletedDoor.TransferToNextRoom(playerTransform);
            MapManager.Instance.CurrentDoor = null;
            UI_EventBus.OnToggleChoiceRoomCanvas?.Invoke(); // 창 닫기
        }
    }
}