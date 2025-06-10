using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    // 해당 버튼이 포함된 최상위 Canvas 비활성화 버튼
    public class CloseBtn : MonoBehaviour
    {
        Canvas _closeTargetCanvas;
        Button _btn;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _closeTargetCanvas = transform.root.GetComponentInChildren<Canvas>();
            _btn.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            _closeTargetCanvas.enabled = false;
        }
    }
}