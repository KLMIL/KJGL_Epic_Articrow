using UnityEngine;
using UnityEngine.UI;
using YSJ;

namespace BMC
{
    public class ExitToTitleBtn : MonoBehaviour
    {
        //[SerializeField] Define.SceneType _sceneType;
        [SerializeField] string _sceneName; // 나중에 Enum으로 바꾸기
        Button _btn;

        static Coroutine _loadSceneCoroutine;

        void Awake()
        {
            _loadSceneCoroutine = null;
        }

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
            //Managers.Scene.LoadScene(_sceneType);

            AnalyticsManager.Instance.SendAnalytics(); // 게임 종료 시 통계 데이터 전송

            GameFlowManager.Instance.Init();
            if (_loadSceneCoroutine == null)
            {
                int index = Managers.Scene.GetBuildIndexBySceneName(_sceneName);
                _loadSceneCoroutine = StartCoroutine(Managers.Scene.LoadSceneCoroutine(index));
            }
        }
    }
}