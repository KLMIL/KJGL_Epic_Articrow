using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YSJ;

namespace BMC
{
    public enum KeyAction
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Cast,
        Interact,
        Inventroy,
        LeftHand,
        RightHand,
        Roll,
    }

    public class RebindBtn : MonoBehaviour
    {
        [SerializeField] KeyAction _keyAction;      // 담당하는 키 액션
        [SerializeField] Button _btn;
        [SerializeField] TextMeshProUGUI _text;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _btn.onClick.AddListener(OnClicked);
        }

        // 클릭 시 호출
        public void OnClicked()
        {
            _text.text = "Choose a new Button";
            Managers.Input.Rebind(_keyAction, _text);
        }
    }
}