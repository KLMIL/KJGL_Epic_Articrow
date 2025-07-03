using UnityEngine;
using YSJ;
using BMC;

public class InventoryCanvas_YSJ : MonoBehaviour
{
    Canvas canvas;

    public Inventory_YSJ inventory { get; private set; }
    public ArtifactWindow_YSJ ArtifactWindow { get; set; }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        inventory = GetComponentInChildren<Inventory_YSJ>();
    }

    private void Start()
    {
        Managers.Input.OnInventoryAction += CanvasOnOff;
        Managers.UI.InventoryCanvas = this;

        if(TutorialManager.Instance != null)
            TutorialManager.Instance.OnEquipPartsAction = CanvasEnable;

    }

    #region 캔버스 기능들
    void CanvasOnOff() 
    {
        canvas.enabled = canvas.enabled ? false : true; 
    }

    public void CanvasEnable(bool value)
    {
        canvas.enabled = value;
        GameManager.Instance.TogglePauseGame(value);
        Debug.Log("인벤토리 캔버스 enable: " + value);
    }

    #endregion


    private void OnDestroy()
    {
        Managers.Input.OnInventoryAction -= CanvasOnOff;
        Managers.UI.InventoryCanvas = null;
    }
}
