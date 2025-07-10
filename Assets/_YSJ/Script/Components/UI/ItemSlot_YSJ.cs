using UnityEngine;
using UnityEngine.EventSystems;
using YSJ;

public class ItemSlot_YSJ : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CanDragItem_YSJ draggedItem = eventData.pointerDrag.GetComponent<CanDragItem_YSJ>();

        if (transform.childCount == 0)
        {
            AddPartsInCurrentSlot(draggedItem); // 슬롯에 아이템이 없으면 그냥 넣어주기
        }
        else
        {
            SwitchItem(draggedItem);
        }
    }

    virtual public void SwitchItem(CanDragItem_YSJ draggedItem) 
    {
        MoveToSlot(draggedItem); // 현재 슬롯에 있는 아이템을 드래그한 애의 부모로 옮겨주고
        AddPartsInCurrentSlot(draggedItem); // 드래그한 애를 현재 슬롯에 넣어주기
    }

    virtual public void MoveToSlot(CanDragItem_YSJ draggedItem)
    {
        CanDragItem_YSJ currentItem = transform.GetChild(0).GetComponent<CanDragItem_YSJ>();
        Transform Goal = draggedItem.currentParent;
        if (currentItem != null)
        {
            // 목표지점이 아티팩트 슬롯이고, 현재 아이템이 파츠면
            if (Goal.TryGetComponent<ArtifactSlotUI_YSJ>(out ArtifactSlotUI_YSJ artifactSlot) && currentItem.TryGetComponent<ImagePartsRoot_YSJ>(out ImagePartsRoot_YSJ parts))
            {
                //print(parts.name);
                artifactSlot.CurrentArtifact.RemoveParts(artifactSlot.SlotIndex); // 현재 아이템이 속한 아티팩트에서 파츠를 제거
                artifactSlot.CurrentArtifact.AddParts(parts, artifactSlot.SlotIndex); // 목표지점 아티팩트슬롯에 이미 들어있던 파츠를 등록
            }
            // 트랜스폼 부모지정
            currentItem.transform.SetParent(Goal);
            currentItem.currentParent = Goal;
        }
    }

    virtual public void AddPartsInCurrentSlot(CanDragItem_YSJ draggedItem) 
    {
        // 드래그한 파츠가 아티팩트 슬롯에서 왔으면
        if (draggedItem.currentParent.TryGetComponent<ArtifactSlotUI_YSJ>(out ArtifactSlotUI_YSJ artifactslot))
        {
            TutorialNotifier();

            // 원래 슬롯에 있던 파츠 아티팩트에서 등록해제
            artifactslot.CurrentArtifact.RemoveParts(artifactslot.SlotIndex);
            artifactslot.CurrentArtifact.UpdateEnhance();
        }
        draggedItem.currentParent = transform;
    }

    #region 튜토리얼 코드
    public void TutorialNotifier()
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.SceneType.TutorialScene)
        {
            BMC.TutorialManager.Instance.IsEquipParts = false; // 파츠를 해제했음을 알림
        }
    }
    #endregion
}