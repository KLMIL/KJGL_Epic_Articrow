using BMC;
using Game.Dungeon;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx
{
    // 현재 씬
    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

    public int GetBuildIndexBySceneName(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return i;
        }

        return -1; // 씬 이름이 빌드 세팅에 없다면 -1
    }

    // 씬 로드(개발 마무리 단계에서 사용할 예정)
    public void LoadScene(SceneType sceneType)
    {
        //Managers.Clear(); // 매니저 정리
        //SceneManager.LoadScene((int)sceneType);

        //나중에 씬 정해지면 이 코드로 사용 예정
        string sceneName = sceneType.ToString();
        SceneManager.LoadScene(sceneName);
    }

    // 개발 과정에서 사용할 로드 씬
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 씬 로드
    public void LoadScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }

    public IEnumerator LoadSceneCoroutine(int idx)
    {
        UI_SceneTransitionCanvas sceneFade = GameObject.FindAnyObjectByType<UI_SceneTransitionCanvas>();
        yield return sceneFade.FadeOutCoroutine(1f); // 페이드 아웃

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(idx);
        asyncOperation.allowSceneActivation = false; // 씬 활성화는 나중에
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f) // 로딩이 거의 끝났을 때
            {
                asyncOperation.allowSceneActivation = true; // 씬 활성화
            }
            yield return null;
        }
    }

    public IEnumerator LoadSceneCoroutine(string sceneName, float duration = 2f)
    {
        // 1. FadeOut 시작
        UI_SceneTransitionCanvas sceneFade = GameObject.FindAnyObjectByType<UI_SceneTransitionCanvas>();
        yield return sceneFade.FadeOutCoroutine(1f); // 페이드 아웃


        // 2. 진행상황 UI 활성화
        UI_StageProgressCanvas stageProgress = GameObject.FindAnyObjectByType<UI_StageProgressCanvas>();
        stageProgress.Show(GameFlowManager.Instance.CurrentRoom - 1, GameFlowManager.Instance.CurrentRoom);


        // 3. 씬 비동기 로드 시작 (즉시 활성화 하지 않음)
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // 씬 활성화는 나중에


        // 4. 연출 애니메이션 실행
        YSJ.Managers.Input.DisablePlayer();
        yield return stageProgress.PlayerProgressAnimation();
        yield return new WaitForSecondsRealtime(0.3f);


        // 5. 씬 비동기 로딩이 끝날 때까지 대기(로딩이 느린 경우 대비)
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }


        // 6. 씬 활성화
        YSJ.Managers.Input.EnablePlayer();
        asyncOperation.allowSceneActivation = true;
        while (!asyncOperation.isDone)
        {
            yield return null;
        }


        // 7. 진행상황 UI 비활성화
        stageProgress.Hide();


        // 8. FadeIn 시작
        sceneFade = GameObject.FindAnyObjectByType<UI_SceneTransitionCanvas>();
        yield return sceneFade.FadeInCoroutine(0.5f);
    }

    // 씬 정리
    public void Clear()
    {
        if (CurrentScene != null) // 조건문 나중에 다시 점검하기
            CurrentScene.Clear();
    }
}