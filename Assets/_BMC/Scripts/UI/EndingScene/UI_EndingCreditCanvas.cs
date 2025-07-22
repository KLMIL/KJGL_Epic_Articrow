using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class UI_EndingCreditCanvas : MonoBehaviour
    {
        [SerializeField] float _scrollSpeed = 40f;           // 크레딧 스크롤 속도

        Canvas _canvas;
        CanvasGroup _canvasGroup;

        EndingCreditContainer _container;            // 크레딧 컨테이너
        RectTransform _containerRectTransform;       // 크레딧 컨테이너의 RectTransform
        Vector3 _bottomLeft;                         // 크레딧 컨테이너의 하단 왼쪽 모서리 위치
        Button _skipBtn;                             // 스킵 버튼

        Coroutine _coroutine;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _container = GetComponentInChildren<EndingCreditContainer>();
            _containerRectTransform = _container.GetComponent<RectTransform>();
            
            _skipBtn = GetComponentInChildren<Button>();
            _skipBtn.onClick.AddListener(OnClickedBtn);
        }

        void Start()
        {
            _canvas.enabled = true;
        }

        void Update()
        {
            if (!_canvas.enabled)
                return;

            CreditScroll();
            //TestCode();
        }

        // 엔딩 크레딧 스크롤
        public void CreditScroll()
        {
            Vector3[] worldCorners = new Vector3[4];
            _containerRectTransform.GetWorldCorners(worldCorners);
            Vector3 bottomLeft = worldCorners[0]; // 하단 왼쪽 모서리

            _containerRectTransform.anchoredPosition += Vector2.up * _scrollSpeed * Time.deltaTime;
            if(bottomLeft.y > Screen.height)
            {
                FadeIn();
            }
        }

        #region 페이드 인
        public void FadeIn()
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(FadeInCoroutine(3f));
            }
        }

        public IEnumerator FadeInCoroutine(float duration)
        {
            _canvas.enabled = true;
            float elapsedTime = 0f;
            float elapsedPercentage = 0f;
            while (elapsedPercentage < 1)
            {
                elapsedPercentage = elapsedTime / duration;
                _canvasGroup.alpha = 1 - elapsedPercentage;
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvas.enabled = false;
        }
        #endregion

        // 엔딩 크레딧 생략 버튼
        void OnClickedBtn()
        {
            FadeIn();
        }

        public void TestCode()
        {
            if (Input.GetKeyDown(KeyCode.T) && _coroutine == null)
            {
                _coroutine = StartCoroutine(FadeInCoroutine(5f));
            }
        }
    }
}