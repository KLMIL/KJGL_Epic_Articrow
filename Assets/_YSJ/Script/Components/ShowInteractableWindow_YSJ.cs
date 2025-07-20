using BMC;
using TMPro;
using UnityEngine;
using YSJ;

public class ShowInteractableWindow_YSJ : MonoBehaviour
{
    [SerializeField] Vector2 _fPosition;
    InteractWindow _interactWindow;

    void Awake()
    {
        GameObject window = Resources.Load<GameObject>("F");
        window = Instantiate(window, transform);
        window.transform.localPosition = _fPosition;
        _interactWindow = window.GetComponent<InteractWindow>();
    }

    public void ShowWindow()
    {            
        _interactWindow.ShowWindow();
    }

    public void HideWindow()
    {
        _interactWindow.HideWindow();
    }
}