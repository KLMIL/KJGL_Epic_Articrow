using UnityEngine;
using static Define;
using YSJ;

namespace BMC
{
    public class TutorialScene : BaseScene
    {
        public override void Init()
        {
            SceneType = SceneType.TutorialScene;
            Managers.Sound.PlayBGM(BGM.InGameScene);
            Time.timeScale = 1f;
            Managers.Input.SetTutorialMode();
            //Debug.Log("튜토리얼 씬 초기화");
        }

        public override void Clear()
        {
            //Debug.Log("튜토리얼 씬 종료");
        }

        void OnDestroy()
        {
            
        }
    }
}