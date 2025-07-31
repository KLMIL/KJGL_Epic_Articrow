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
            //AnalyticsManager.Instance.SendAnalytics(); // 게임 종료 시 통계 데이터 전송

            PlayerManager.Instance.Clear();

            GameFlowManager.Instance.InitRetry();

            YSJ.Managers.Scene.LoadScene("StageStartScene");
        }
    }
}