using UnityEngine;

/// <summary>
/// 설정 캔버스들의 부모 오브젝트에 들어갈 클래스
/// </summary>
public class SettingsPanelCanvases : MonoBehaviour
{
    Canvas[] _canvasArray;
    void Start()
    {
        _canvasArray = gameObject.GetComponentsInDirectChildren<Canvas>();
        UI_TitleEventBus.OnActivePanelCanvas += ActivePanelCanvas;
    }

    public void ActivePanelCanvas(int idx)
    {
        foreach (Canvas canvas in _canvasArray)
        {
            canvas.enabled = false;
        }
        _canvasArray[idx].enabled = true;
    }
}