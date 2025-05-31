using System;
using UnityEngine;

namespace YSJ
{
    public class UIManager
    {
        public event Action<GameObject> OnUI_AddInventoryEvent;
        public void UI_AddInventory(GameObject item)
        {
            OnUI_AddInventoryEvent?.Invoke(item);
        }

        public Transform LeftHand;
        public Transform RightHand;
    }
}