using System;
using UnityEngine;

namespace YSJ
{
    public class UIManager
    {
        public ActionT1<GameObject> OnAddInventorySlotActionT1 = new();

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