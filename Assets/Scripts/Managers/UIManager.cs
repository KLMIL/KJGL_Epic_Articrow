using BMC;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace YSJ
{
    public class UIManager
    {
        public UI_SettingsCanvas SettingsCanvas;
        public SettingsCloseBtn SettingsCloseBtn;

        public InventoryCanvas_YSJ InventoryCanvas;

        public void InstantiateSettingsUI()
        {
            UI_CommonEventBus.Clear();
            GameObject settingsUI = Managers.Resource.Load<GameObject>("UI/UI_SettingsCanvas");
            GameObject.Instantiate(settingsUI, Managers.Instance.transform);
        }

        /// <summary>
        /// ManaHeart에서 구독, Artifact_YSJ에서 호출
        /// </summary>
        public Action OnManaLackEvent = null;

        /// <summary>
        /// ManaHeart에서 구독, Artifact_YSJ에서 호출
        /// </summary>
        public Action OnStopManaLackEvent = null;
    }
}