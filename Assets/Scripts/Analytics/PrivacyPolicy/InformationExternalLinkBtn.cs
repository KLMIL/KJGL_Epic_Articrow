using UnityEngine;
using UnityEngine.UI;

public class InformationExternalLinkBtn : MonoBehaviour
{
    Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        Debug.Log("로그 수집 링크");
        Application.OpenURL("https://docs.unity.com/ugs/en-us/manual/analytics/manual/privacy-overview");
        Application.OpenURL("https://gist.githubusercontent.com/bokob/e3fc4448499795397c796b8bb90f964e/raw/b065d6cb97668bc16b22cbe19fa1c8be71e577e7/Information%2520Collection%2520items");
    }
}