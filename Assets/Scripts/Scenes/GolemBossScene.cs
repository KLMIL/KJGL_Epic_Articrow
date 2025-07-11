using UnityEngine;
using static Define;
using YSJ;

public class GolemBossScene : BaseScene
{
    public override void Init()
    {
        SceneType = SceneType.GolemBossScene;
        Managers.Sound.PlayBGM(BGM.GolemBossScene);
        Time.timeScale = 1f;
        Managers.Input.SetGameMode();
    }

    public override void Clear()
    {
        // 씬 정리
        //Managers.Sound.Stop();
        Debug.Log("인게임 씬 정리");
    }

    void OnDestroy()
    {
        Managers.Input.ClearAction();
    }
}
