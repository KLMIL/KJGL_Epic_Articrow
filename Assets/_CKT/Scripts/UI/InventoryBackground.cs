using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBackground : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject pointerDrag = eventData.pointerDrag;
        //Background 위에서 Drop했을 때만 호출
        if ((pointerDrag != null) && (pointerDrag.transform.parent == this.transform.parent))
        {
            //TODO : 이전에 있었던 슬롯에 다시 추가하기
            pointerDrag.GetComponent<IAddSlotable>().AddSlot(null);
        }
    }
}
