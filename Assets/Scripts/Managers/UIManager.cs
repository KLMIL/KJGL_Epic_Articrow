using System;
using UnityEngine;

namespace YSJ
{
    public class UIManager
    {
        public event Action<GameObject> OnUI_AddInventorySlotEvent;
        public void UI_AddInventory(GameObject item)
        {
            OnUI_AddInventorySlotEvent?.Invoke(item);
        }

        public Transform LeftHand;
        public Transform RightHand;
    }
}