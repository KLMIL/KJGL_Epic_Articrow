using UnityEngine;

namespace YSJ
{
    public class UIManager
    {
        public InventoryCanvas_YSJ InventoryCanvas;

        public void InstantiateSettingsUI()
        {
            UI_CommonEventBus.Clear();
            GameObject settingsUI = Managers.Resource.Load<GameObject>("UI/UI_SettingsCanvas");
            GameObject.Instantiate(settingsUI, Managers.Instance.transform);
        }
    }
}