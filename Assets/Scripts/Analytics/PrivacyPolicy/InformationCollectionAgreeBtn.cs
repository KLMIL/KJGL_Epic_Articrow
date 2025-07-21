using UnityEngine;
using UnityEngine.UI;

public class InformationCollectionAgreeBtn : MonoBehaviour
{
    Button _btn;
    Canvas _canvas;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _canvas = GetComponentInParent<Canvas>();
        _btn.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        PlayerPrefs.SetInt("InformationCollection", 1);
        PlayerPrefs.Save();
        Debug.Log("플레이 데이터 수집 동의");

        // TODO: 통계 분석 시스템 활성화
        AnalyticsManager.Instance.AnalyticsInit();

        _canvas.gameObject.SetActive(false);
    }
}