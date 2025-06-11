using System;
using UnityEngine;

namespace YSJ
{
    public class UIManager
    {
        event Action<GameObject> OnAddInventorySlotEvent;
        public void SubAddInventorySlot(Action<GameObject> newSub)
        {
            OnAddInventorySlotEvent = newSub;
        }
        public void InvokeAddInventorySlot(GameObject item)
        {
            OnAddInventorySlotEvent?.Invoke(item);
        }

        public Transform LeftHand;
        public Transform RightHand;

        public void InstantiateSettingsUI()
        {
            UI_CommonEventBus.Clear();
            GameObject settingsUI = Managers.Resource.Load<GameObject>("UI/UI_SettingsCanvas");
            GameObject.Instantiate(settingsUI, Managers.Instance.transform);
        }
    }
}