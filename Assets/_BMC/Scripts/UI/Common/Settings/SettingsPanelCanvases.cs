using UnityEngine;
using YSJ;

/// <summary>
/// 설정 캔버스들의 부모 오브젝트에 들어갈 클래스
/// </summary>
public class SettingsPanelCanvases : MonoBehaviour
{
    Canvas[] _canvasArray;

    void Awake()
    {
        _canvasArray = gameObject.GetComponentsInDirectChildren<Canvas>();
        //UI_TitleEventBus.OnActivePanelCanvas += ActivePanelCanvas;
        UI_CommonEventBus.OnActivePanelCanvas += ActivePanelCanvas;
        UI_CommonEventBus.OnDeactivatePanelCanvas += DeactivatePanelCanvas;
    }

    void Start()
    {
        //_canvasArray = gameObject.GetComponentsInDirectChildren<Canvas>();
        ////UI_TitleEventBus.OnActivePanelCanvas += ActivePanelCanvas;
        //UI_CommonEventBus.OnActivePanelCanvas += ActivePanelCanvas;
    }

    public void ActivePanelCanvas(int idx)
    {
        //if (_canvasArray == null)
        //    Debug.Log("설정창에서 캔버스가 널");
        //else
        //    Debug.Log(_canvasArray.Length);

        DeactivatePanelCanvas();
        _canvasArray[idx].enabled = true;
    }

    public void DeactivatePanelCanvas()
    {
        // 키 확정 캔버스 On
        if (Managers.UI.KeyConfirmationCanvas.Canvas.enabled)
            return;

        foreach (Canvas canvas in _canvasArray)
        {
            canvas.enabled = false;
        }
    }
}