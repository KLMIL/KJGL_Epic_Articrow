using BMC;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneManagerEx
{
    // 현재 씬
    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

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

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        //Debug.Log("왜 안돼");

        UI_SceneTransitionCanvas sceneFade = GameObject.FindAnyObjectByType<UI_SceneTransitionCanvas>();
        yield return sceneFade.FadeOutCoroutine(1f); // 페이드 아웃

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false; // 씬 활성화는 나중에
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f) // 로딩이 거의 끝났을 때
            {
                asyncOperation.allowSceneActivation = true; // 씬 활성화
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    // 씬 정리
    public void Clear()
    {
        if (CurrentScene != null) // 조건문 나중에 다시 점검하기
            CurrentScene.Clear();
    }
}