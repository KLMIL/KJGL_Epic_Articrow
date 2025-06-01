using UnityEngine;

public class SettingsPanels : MonoBehaviour
{
    void Start()
    {
        UI_TitleEventBus.OnActivePanel += ActivePanel;
    }

    public void ActivePanel(int idx)
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