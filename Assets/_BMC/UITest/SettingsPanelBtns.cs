using UnityEngine;

public class SettingsPanelBtns : MonoBehaviour
{
    void Start()
    {
        UI_TitleEventBus.OnActivePanel += ActiveBtn;
    }

    public void ActiveBtn(int idx)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }

        transform.GetChild(idx).gameObject.SetActive(true);
    }
}
