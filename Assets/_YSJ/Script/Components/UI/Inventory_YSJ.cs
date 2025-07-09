using UnityEngine;
using UnityEngine.UI;

public class Inventory_YSJ : MonoBehaviour
{
    // bool값은 인벤토리 넣었는지 성공여부
    public bool TryAddItem(GameObject Item) 
    {
        foreach (Transform slot in transform) 
        {
            if (slot.childCount == 0) 
            {
                GameObject spawnedItem = Instantiate(Item, slot.transform);
                if (spawnedItem.TryGetComponent<Image>(out Image image)) 
                {
                    image.raycastTarget = true;
                }

                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        foreach (Transform slot in transform)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }
}
