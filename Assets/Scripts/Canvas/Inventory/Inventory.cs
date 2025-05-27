using UnityEngine;

public class Inventory : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.inventory = this;
    }

    public bool TryGetitem(Item item) 
    {
        GameObject inventoryItem = item.GetSkillObject();

        foreach (Transform slot in transform)
        {
            if (slot.childCount == 0) 
            {
                GameObject spawnedObj = Instantiate(inventoryItem, slot);
                return true;
            }
        }
        return false;
    }

    public void DeleteAllItem() 
    {
        foreach (Transform slot in transform)
        {
            if (slot.childCount != 0)
            {
                foreach (Transform skill in slot) 
                {
                    Destroy(skill.gameObject);
                }
            }
        }
    }
}
