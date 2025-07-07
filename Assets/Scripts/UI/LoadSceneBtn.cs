using UnityEngine;
using UnityEngine.UI;

namespace YSJ
{
    public class LoadSceneBtn : MonoBehaviour
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
            //Managers.Scene.LoadScene(_sceneName);
            GameFlowManager.Instance.Init();
            if(_loadSceneCoroutine == null)
            {
                _loadSceneCoroutine = StartCoroutine(Managers.Scene.LoadSceneCoroutine(_sceneName));
            }
        }
    }
}