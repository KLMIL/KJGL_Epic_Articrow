using UnityEngine;
using static Define;

namespace YSJ
{
    public class EndingScene : BaseScene
    {
        public override void Init()
        {
            SceneType = SceneType.EndingScene;
            Managers.Sound.PlayBGM(BGM.EndingScene);
            //Debug.Log("엔드 씬 초기화");
            AnalyticsManager.Instance.SendAnalytics(); // 엔딩 씬 진입 시 통계 데이터 전송
        }

        public override void Clear()
        {
            //Debug.Log("엔딩 씬 종료");

            // 이벤트 버스 초기화
            UI_InGameEventBus.Clear();
        }
    }
}