using UnityEngine;
using UnityEngine.UI;
using YSJ;

namespace BMC
{
    public class ContinueBtn : MonoBehaviour
    {
        Button _btn;

        void Awake()
        {
            _btn = GetComponent<Button>();
        }

        void Start()
        {
            _btn.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            Managers.Input.OnPauseAction?.Invoke();
            Managers.Input.SetGameMode();
        }
    }
}