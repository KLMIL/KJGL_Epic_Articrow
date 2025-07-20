using UnityEngine;
using UnityEngine.UI;


public class InformationCollectionDenyBtn : MonoBehaviour
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
        PlayerPrefs.SetInt("InformationCollection", 0);
        PlayerPrefs.Save();
        Debug.Log("플레이 데이터 수집 거부");

        // TODO: 통계 분석 시스템 비활성화

        _canvas.gameObject.SetActive(false);
    }
}
