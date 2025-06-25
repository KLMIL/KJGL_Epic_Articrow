using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot_YSJ : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CanDragItem_YSJ DraggedItem = eventData.pointerDrag.GetComponent<CanDragItem_YSJ>();

        if (transform.childCount == 0)
        {
            DraggedItem.currentParent = transform;
        }
        else
        {
            // 아이템이 이미 존재하면 부모바꿔주기
            CanDragItem_YSJ currentItem = transform.GetChild(0).GetComponent<CanDragItem_YSJ>();
            if (currentItem != null) 
            {
                currentItem.transform.SetParent(DraggedItem.currentParent);
                currentItem.currentParent = DraggedItem.currentParent;

                DraggedItem.currentParent = transform;
            }
            
        }
    }
}
