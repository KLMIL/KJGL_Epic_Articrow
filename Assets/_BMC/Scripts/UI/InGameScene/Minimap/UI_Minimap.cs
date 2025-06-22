using System.Collections.Generic;
using UnityEngine;
using static Define;
using YSJ;

namespace BMC
{
    public class UI_Minimap : MonoBehaviour
    {
        UI_Room[] _ui_Rooms;

        Dictionary<RoomState, Color> _roomStateColorDict = new Dictionary<RoomState, Color>
        {
            { RoomState.Undiscover, new Color(1f, 1f, 1f, 0f) },         // 미발견
            { RoomState.Deactivate, new Color(0.5f, 0.5f, 0.5f, 0.5f) }, // 비활성화
            { RoomState.Active, Color.white }                            // 활성화
        };

        // 최적화 꼭 필요 (임시)
        Dictionary<RoomType, Sprite> _roomTypeIconDict = new Dictionary<RoomType, Sprite>();

        void Awake()
        {
            _ui_Rooms = GetComponentsInChildren<UI_Room>();

            UI_InGameEventBus.OnActiveMinimapRoom += SetActiveRoom;
            UI_InGameEventBus.OnDeactivateMinimapRoom += SetDeactivateRoom;

            // 아이콘 로드 및 캐싱
            Sprite[] minimapIcons = Managers.Resource.LoadAll<Sprite>("UI/Minimap");
            foreach (Sprite minimapIcon in minimapIcons)
            {
                if (System.Enum.IsDefined(typeof(RoomType), minimapIcon.name))
                {
                    RoomType roomType = Util.StringToEnum<RoomType>(minimapIcon.name);
                    _roomTypeIconDict.Add(roomType, minimapIcon);
                }
            }
        }

        public void SetActiveRoom(int idx)
        {
            _ui_Rooms[idx].SetRoomColor(_roomStateColorDict[RoomState.Active]);
            SetRoomIcon(idx);
        }

        public void SetDeactivateRoom(int idx)
        {
            _ui_Rooms[idx].SetRoomColor(_roomStateColorDict[RoomState.Deactivate]);
        }

        public void SetRoomIcon(int idx)
        {
            // 해당하는 인덱스의 방 정보 가져오기
            int row = idx / MapManager.Instance.MaxCol;
            int col = idx % MapManager.Instance.MaxCol;
            Room room = MapManager.Instance.GetRoom(row, col);

            // 아이콘 설정
            Sprite iconSprite = null;
            _roomTypeIconDict.TryGetValue(room.RoomData.RoomType, out iconSprite);
            _ui_Rooms[idx].SetRoomIcon(iconSprite);
        }
    }
}