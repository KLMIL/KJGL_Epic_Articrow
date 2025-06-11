using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class KeyResetBtn : MonoBehaviour
    {
        Button _btn;
        void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClicked);
        }

        void OnClicked()
        {
            //UI_TitleEventBus.OnResetKeyBind?.Invoke();
            UI_CommonEventBus.OnResetKeyBind?.Invoke();
        }
    }
}