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
            Debug.Log("엔드 씬 초기화");
        }

        public override void Clear()
        {
            Debug.Log("엔딩 씬 종료");
        }
    }
}