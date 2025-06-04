using System;
using UnityEngine;

namespace CKT
{
    public class UIManager
    {
        public event Action<GameObject> OnUI_AddInventoryEvent;
        public void UI_AddInventory(GameObject item)
        {
            OnUI_AddInventoryEvent?.Invoke(item);
        }
    }
}