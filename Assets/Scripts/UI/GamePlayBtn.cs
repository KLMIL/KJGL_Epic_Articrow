using UnityEngine;
using UnityEngine.UI;

namespace YSJ
{
    public class GamePlayBtn : MonoBehaviour
    {
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
            int sceneIdx = (!Managers.Data.IsClearTutorial) ? 1 : 2;
            if (_loadSceneCoroutine == null)
            {
                _loadSceneCoroutine = StartCoroutine(Managers.Scene.LoadSceneCoroutine(sceneIdx));
            }
        }
    }
}