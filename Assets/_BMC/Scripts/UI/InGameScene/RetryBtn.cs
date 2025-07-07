using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BMC
{
    public class RetryBtn : MonoBehaviour
    {
        Button _btn;

        void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClicked);
        }

        void OnClicked()
        {
            PlayerManager.Instance.Clear();

            GameFlowManager.Instance.InitRetry();

            YSJ.Managers.Scene.LoadScene("StageStartScene");
        }
    }
}