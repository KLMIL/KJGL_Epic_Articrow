using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot_YSJ : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CanDragItem_YSJ draggedItem = eventData.pointerDrag.GetComponent<CanDragItem_YSJ>();

        if (transform.childCount == 0)
        {
            AddCurrentSlot(draggedItem); // 슬롯에 아이템이 없으면 그냥 넣어주기
        }
        else
        {
            SwitchItem(draggedItem);
        }
    }

    virtual public void SwitchItem(CanDragItem_YSJ draggedItem) 
    {
        MoveToSlot(draggedItem.currentParent); // 현재 드래그한 아이템의 부모를 슬롯으로 이동
        AddCurrentSlot(draggedItem); // 드랍한 슬롯에 새로 드래그한 아이템을 넣어주기
    }

    virtual public void MoveToSlot(Transform Goal)
    {
        CanDragItem_YSJ currentItem = transform.GetChild(0).GetComponent<CanDragItem_YSJ>();
        if (currentItem != null)
        {
            currentItem.transform.SetParent(Goal);
            currentItem.currentParent = Goal;
        }
    }

    virtual public void AddCurrentSlot(CanDragItem_YSJ draggedItem) 
    {
        // 아티팩트 슬롯에서 왔으면
        if (draggedItem.currentParent.TryGetComponent<ArtifactSlotUI_YSJ>(out ArtifactSlotUI_YSJ artifactslot)) 
        {
            // 원래 슬롯에 있던 파츠 아티팩트에서 등록해제
            artifactslot.CurrentArtifact.RemoveParts(artifactslot.SlotIndex); 
        }
        draggedItem.currentParent = transform;
    }
}
