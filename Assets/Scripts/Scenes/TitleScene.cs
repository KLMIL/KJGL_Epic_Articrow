using UnityEngine;

namespace YSJ
{
    public class TitleScene : BaseScene
    {
        public override void Init()
        {
            SceneType = Define.SceneType.TitleScene;
            Managers.Sound.PlayBGM(Define.BGM.TitleScene);
            Time.timeScale = 1f;
            Debug.Log("타이틀 씬 초기화");
        }

        public override void Clear()
        {
            Debug.Log("타이틀 씬 종료");
        }

        void OnDestroy()
        {
            // TODO 나중에 설정 창도 인게임에서 열 수 있게 하려면 이 부분 꼭 수정해야 함
            // 타이틀 씬 종료 시 이벤트 버스 초기화
            UI_TitleEventBus.Clear();
        }
    }
}