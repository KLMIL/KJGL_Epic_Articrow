using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YSJ;

public class CanDragItem_YSJ : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Transform currentParent;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        currentParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas) 
        {
            transform.SetParent(parentCanvas.transform);
        }
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(currentParent);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 우클릭 로직
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Debug.Log("Right Click Down");

            // 아티팩트 슬롯 -> 인벤토리 슬롯
            if (transform.parent.TryGetComponent<ArtifactSlotUI_YSJ>(out ArtifactSlotUI_YSJ artifactSlot)) // 지금 있는 곳이 아티팩트 슬롯이면
            {
                if (Managers.UI.InventoryCanvas.inventory.TryAddItem(gameObject)) // 인벤토리에 이 아이템 추가 시도
                {
                    artifactSlot.CurrentArtifact.RemoveParts(artifactSlot.SlotIndex); // 추가 성공했으면 현재 아티팩트에서 파츠 해제
                    Destroy(gameObject); // UI에 남아있는 파츠 지워줌(나)
                }
            }
            // 인벤토리 슬롯 -> 아티팩트 슬롯
            else if (transform.parent.TryGetComponent<ItemSlot_YSJ>(out ItemSlot_YSJ itemSlot)) 
            {
                foreach (Transform slot in Managers.UI.InventoryCanvas.ArtifactWindow.ArtifactSlotWindow) // 아티팩트 슬롯에 자리있는지 한바퀴 돌기
                {
                    if (slot.childCount == 0) // 어? 자리가 남네?
                    {
                        //print(slot.name);

                        ArtifactSlotUI_YSJ targetArtifactSlot = slot.GetComponent<ArtifactSlotUI_YSJ>(); // 자리남는 아티팩트 슬롯 저장해서
                        targetArtifactSlot.AddPartsInCurrentSlot(this); // 이 이미지 파츠 현재 아티팩트에 등록
                        Managers.UI.InventoryCanvas.ArtifactWindow.ArtifactWindowUpdate(targetArtifactSlot.CurrentArtifact); // 아티팩트 슬롯에 장착시키고 나서 UI갱신
                        Destroy(gameObject); // UI에 남아있는 나는 지워줌
                        break; // 추가 완료했으니 for문 탈출
                    }
                }
            }
        }
    }
}
