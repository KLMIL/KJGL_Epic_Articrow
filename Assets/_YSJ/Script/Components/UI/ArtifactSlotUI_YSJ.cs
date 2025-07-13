using BMC;
using UnityEngine;
using UnityEngine.EventSystems;
using YSJ;

public class ArtifactSlotUI_YSJ : ItemSlot_YSJ
{
    public Artifact_YSJ CurrentArtifact; // 이 슬롯에 장착될 아티팩트
    public int SlotIndex; // 슬롯의 인덱스

    public override void MoveToSlot(CanDragItem_YSJ draggedItem)
    {
        CurrentArtifact.RemoveParts(SlotIndex); // 현재 슬롯에 있는 파츠를 아티팩트에서 제거

        base.MoveToSlot(draggedItem);
    }

    public override void MoveToSlot(Transform Goal)
    {
        CurrentArtifact.RemoveParts(SlotIndex); // 현재 슬롯에 있는 파츠를 아티팩트에서 제거

        base.MoveToSlot(Goal);
    }


    public override void AddPartsInCurrentSlot(CanDragItem_YSJ draggedItem)
    {
        base.AddPartsInCurrentSlot(draggedItem);

        ImagePartsRoot_YSJ imageParts = draggedItem.GetComponent<ImagePartsRoot_YSJ>();
        if (imageParts)
        {
            CurrentArtifact.AddParts(imageParts, SlotIndex); // 현재 슬롯의 파츠 아티팩트에 등록
            TutorialNotifier();
        }
        else 
        {
            print("이거 파츠 아닌디?");
        }
    }

    #region 튜토리얼 전용 코드
    public void TutorialNotifier()
    {
        if (Managers.Scene.CurrentScene.SceneType == Define.SceneType.TutorialScene)
        {
            TutorialManager.Instance.IsEquipParts = true;               // 파츠를 장착했음을 알림
            TutorialManager.Instance.OnEquipPartsAction.Invoke(false);  // 시간 정지 해제
            TutorialManager.Instance.TutorialInput.EnableActionMap();   // 튜토리얼 입력 활성화
        }
    }
    #endregion
}