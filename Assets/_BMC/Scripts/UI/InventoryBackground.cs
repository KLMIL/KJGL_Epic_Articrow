using BMC;
using UnityEngine;
using UnityEngine.EventSystems;
using YSJ;

/// <summary>
/// 인벤토리 배경. 아이템을 드롭하면 UI에서 제거하고 플레이어 주변에 해당하는 파츠 소환
/// </summary>
public class InventoryBackground : MonoBehaviour, IDropHandler
{
    // 인벤토리 배경에 드롭 시 동작
    public void OnDrop(PointerEventData eventData)
    {
        CanDragItem_YSJ draggedItem = eventData.pointerDrag.GetComponent<CanDragItem_YSJ>();
        if(draggedItem != null)
        {
            Debug.LogError($"OnDrop: {draggedItem.name} on {gameObject.name}");

            string partName = draggedItem.GetComponent<ImagePartsRoot_YSJ>().partsName;

            // TODO: 인벤토리에서 버린 파츠를 플레이어 주변에 소환시키기
            SpawnPart(partName);

            Destroy(draggedItem.gameObject); // 드래그한 아이템을 제거
        }
    }

    void SpawnPart(string partName)
    {
        // 플레이어 주변에 랜덤한 방향으로 해당하는 파츠 소환

        // 테스트 코드
        GameObject partPrefab = Managers.Resource.Load<GameObject>($"NewImageParts/ImageParts_{partName}");

        if (partPrefab == null)
        {
            Debug.LogError($"못찾음: {partName}");
            return;
        }

        GameObject partInstance = Instantiate(partPrefab, PlayerManager.Instance.transform.position + Random.insideUnitSphere, Quaternion.identity);
        Debug.LogError($"{partInstance} 버림");
    }
}