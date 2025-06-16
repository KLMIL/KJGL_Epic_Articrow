using System.Collections;
using UnityEngine;

namespace BMC
{
    public class UI_BloodCanvas : MonoBehaviour
    {
        Canvas _canvas;
        float _showTime = 2f;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            UI_InGameEventBus.OnShowBloodCanvas += ShowCanvas;
        }

        void Start()
        {
            _canvas.enabled = false; // 초기에는 캔버스를 비활성화
        }

        void ShowCanvas()
        {
            StartCoroutine(ShowCanvasCoroutine());
        }

        IEnumerator ShowCanvasCoroutine()
        {
            _canvas.enabled = true;
            yield return new WaitForSeconds(_showTime);
            _canvas.enabled = false;
        }

        void OnDestroy()
        {
            UI_InGameEventBus.OnShowBloodCanvas -= ShowCanvas;
        }
    }
}