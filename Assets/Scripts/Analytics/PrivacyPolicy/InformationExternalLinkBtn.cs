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
    }
}