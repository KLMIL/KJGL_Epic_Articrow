using UnityEngine;
using YSJ;

public class InventoryCanvas_YSJ : MonoBehaviour
{
    Canvas canvas;

    public Inventory_YSJ inventory { get; private set; }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        inventory = GetComponentInChildren<Inventory_YSJ>();
    }

    private void Start()
    {
        Managers.Input.OnInventoryAction += CanvasOnOff;
        Managers.UI.InventoryCanvas = this;
    }

    #region 캔버스 기능들
    void CanvasOnOff() 
    {
        canvas.enabled = canvas.enabled ? false : true; 
    }

    #endregion


    private void OnDestroy()
    {
        Managers.Input.OnInventoryAction -= CanvasOnOff;
        Managers.UI.InventoryCanvas = null;
    }
}
