using UnityEngine;
using UnityEngine.EventSystems;

public class ArtifactSlotUI_YSJ : ItemSlot_YSJ
{
    public Artifact_YSJ CurrentArtifact; // 이 슬롯에 장착될 아티팩트
    public int SlotIndex; // 슬롯의 인덱스

    public override void MoveToSlot(Transform Goal)
    {
        CurrentArtifact.RemoveParts(SlotIndex); // 현재 슬롯의 파츠 아티팩트에서 등록해제

        base.MoveToSlot(Goal);
    }
    public override void AddCurrentSlot(CanDragItem_YSJ draggedItem)
    {
        int beforeIndex = SlotIndex;

        base.AddCurrentSlot(draggedItem);

        ImagePartsRoot_YSJ imageParts = draggedItem.GetComponent<ImagePartsRoot_YSJ>();
        if (imageParts)
        {
            if (beforeIndex < SlotIndex)
            {
                CurrentArtifact.AddParts(imageParts, SlotIndex + 1); // 현재 슬롯의 파츠 아티팩트에 등록
            }
            else 
            {
                CurrentArtifact.AddParts(imageParts, SlotIndex); // 현재 슬롯의 파츠 아티팩트에 등록
            }
        }
        else 
        {
            print("이거 파츠 아닌디?");
        }
    }
}
