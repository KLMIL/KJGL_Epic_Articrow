using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BMC
{
    public class UI_GameOverCanvas : MonoBehaviour
    {
        Canvas _canvas;
        Image _blackScreenPanel;
        TextMeshProUGUI _gameOverText;
        VerticalLayoutGroup _verticalLayoutGroup;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _blackScreenPanel = GetComponentInChildren<Image>();
            _gameOverText = GetComponentInChildren<TextMeshProUGUI>();
            _verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
            _verticalLayoutGroup.gameObject.SetActive(false);

            UI_InGameEventBus.OnShowGameOverCanvas += ShowGameOver;
        }

        public void ShowGameOver()
        {
            StartCoroutine(ShowGameOverCoroutine());
        }

        IEnumerator ShowGameOverCoroutine()
        {
            _canvas.enabled = true;
            _blackScreenPanel.color = new Color(0, 0, 0, 0);
            _gameOverText.color = new Color(1, 0, 0, 0);

            // 배경
            float alphaValue = 0;
            for (alphaValue = 0; alphaValue < 1; alphaValue += Time.deltaTime)
            {
                _blackScreenPanel.color = new Color(0, 0, 0, alphaValue);
                yield return null;
            }
            _blackScreenPanel.color = Color.black;

            // 텍스트
            for (alphaValue = 0; alphaValue < 1; alphaValue += Time.deltaTime)
            {
                _gameOverText.color = new Color(1, 0, 0, alphaValue);
                yield return null;
            }
            _gameOverText.color = Color.red;
            _verticalLayoutGroup.gameObject.SetActive(true);
        }

        void OnDestroy()
        {
            UI_InGameEventBus.OnShowGameOverCanvas -= ShowGameOver;
        }
    }
}