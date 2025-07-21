using UnityEngine;
using UnityEngine.UI;

public class UI_InformationCollectionCanvas : MonoBehaviour
{
    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        // PlayerPref에서 플레이 데이터 수집 동의를 했는지 검사
        bool isExist = PlayerPrefs.HasKey("InformationCollection");

        if (isExist)
        {
            int informationCollection = PlayerPrefs.GetInt("InformationCollection", 0);
            if (informationCollection == 1)
            {
                // TODO: 통계 프로그램 활성화
            }
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("InformationCollection", 0);
            PlayerPrefs.Save();
        }
    }
}