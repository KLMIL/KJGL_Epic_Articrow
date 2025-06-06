using UnityEngine;
using UnityEngine.UI;
using YSJ;

namespace BMC
{
    public class ContinueBtn : MonoBehaviour
    {
        Button _btn;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClicked);
        }

        void OnClicked()
        {
            Managers.Input.OnPauseAction?.Invoke();
            Managers.Input.SetGameMode();
        }
    }
}