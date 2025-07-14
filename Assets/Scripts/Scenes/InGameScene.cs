using UnityEngine;
using static Define;

namespace YSJ
{
    public class InGameScene : BaseScene
    {
        public override void Init()
        {
            SceneType = SceneType.InGameScene;
            Managers.Sound.PlayBGM(BGM.InGameScene);
            Time.timeScale = 1f;
            Managers.Input.SetGameMode();
            //Debug.Log("인게임 씬 초기화");
        }

        public override void Clear()
        {
            // 씬 정리
            //Managers.Sound.Stop();
            //Debug.Log("인게임 씬 정리");
        }

        void OnDestroy()
        {
            Managers.Input.ClearAction();
        }
    }
}