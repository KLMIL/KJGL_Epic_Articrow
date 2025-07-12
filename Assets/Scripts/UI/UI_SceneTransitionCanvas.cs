using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class UI_SceneTransitionCanvas : MonoBehaviour
    {
        Canvas _canvas;
        Image _sceneFadeImage;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _sceneFadeImage = GetComponentInChildren<Image>();
        }

        void Start()
        {
            StartCoroutine(FadeInCoroutine(1f)); // 시작할 때 페이드 인
        }

        public IEnumerator FadeInCoroutine(float duration)
        {
            if (_canvas == null) yield break;
            _canvas.enabled = true;

            Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1f);
            Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0f);

            yield return FadeCoroutine(startColor, targetColor, duration);

            if (_canvas == null) yield break;
            _canvas.enabled = false;
        }

        public IEnumerator FadeOutCoroutine(float duration)
        {
            if (_canvas == null) yield break;
            _canvas.enabled = true;

            Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0f);
            Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1f);

            yield return FadeCoroutine(startColor, targetColor, duration);
        }

        IEnumerator FadeCoroutine(Color startColor, Color targetColor, float duration)
        {
            float elapsedTime = 0f;
            float elapsedPercentage = 0f;

            while (elapsedPercentage < 1)
            {
                if (_sceneFadeImage == null) yield break;
                elapsedPercentage = elapsedTime / duration;
                _sceneFadeImage.color = Color.Lerp(startColor, targetColor, elapsedPercentage);
                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }
    }
}