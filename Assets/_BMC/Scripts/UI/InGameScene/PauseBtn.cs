using UnityEngine;
using UnityEngine.UI;
using YSJ;

public class PauseBtn : MonoBehaviour
{
    Button _btn;

    void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        Managers.Input.Pause();
    }
}