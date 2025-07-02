using UnityEngine;
using YSJ;

public class ShowInteractableWindow_YSJ : MonoBehaviour
{
    GameObject window;

    void Start()
    {
        window = Resources.Load<GameObject>("F");
        window = Instantiate(window, transform);
        window.SetActive(false);
        window.transform.localPosition = new Vector3(1, 1, 0);
    }

    public void ShowWindow()
    {
        window.SetActive(true);
    }
    public void HideWindow()
    {
        window.SetActive(false);
    }
}
