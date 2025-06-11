using System.Collections.Generic;
using UnityEngine;

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

        void Awake()
        {
            _ui_Rooms = GetComponentsInChildren<UI_Room>();

            UI_InGameEventBus.OnActiveMinimapRoom += SetActiveRoom;
            UI_InGameEventBus.OnDeactivateMinimapRoom += SetDeactivateRoom;
        }

        public void SetActiveRoom(int idx)
        {
            _ui_Rooms[idx].SetRoomColor(_roomStateColorDict[RoomState.Active]);
        }

        public void SetDeactivateRoom(int idx)
        {
            _ui_Rooms[idx].SetRoomColor(_roomStateColorDict[RoomState.Deactivate]);
        }
    }
}