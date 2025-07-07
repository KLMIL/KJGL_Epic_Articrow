using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class ExitToTitleBtn : MonoBehaviour
    {
        //[SerializeField] Define.SceneType _sceneType;
        [SerializeField] string _sceneName; // 나중에 Enum으로 바꾸기
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
            //Managers.Scene.LoadScene(_sceneType);
            GameFlowManager.Instance.Init();
            YSJ.Managers.Scene.LoadScene(_sceneName);
        }
    }
}