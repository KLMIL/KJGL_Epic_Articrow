using UnityEngine;
using UnityEngine.EventSystems;

public class ArtifactSlotUI_YSJ : ItemSlot_YSJ
{
    public Artifact_YSJ CurrentArtifact; // 이 슬롯에 장착될 아티팩트
    public int SlotIndex; // 슬롯의 인덱스

    public override void MoveToSlot(CanDragItem_YSJ draggedItem)
    {
        CurrentArtifact.RemoveParts(SlotIndex); // 현재 슬롯에 있는 파츠를 아티팩트에서 제거

        base.MoveToSlot(draggedItem);
    }

    public override void AddPartsInCurrentSlot(CanDragItem_YSJ draggedItem)
    {
        base.AddPartsInCurrentSlot(draggedItem);

        ImagePartsRoot_YSJ imageParts = draggedItem.GetComponent<ImagePartsRoot_YSJ>();
        if (imageParts)
        {
            CurrentArtifact.AddParts(imageParts, SlotIndex); // 현재 슬롯의 파츠 아티팩트에 등록
        }
        else 
        {
            print("이거 파츠 아닌디?");
        }
    }
}
