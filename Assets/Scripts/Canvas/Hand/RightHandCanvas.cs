using UnityEngine;

public class RightHandCanvas : MonoBehaviour
{
    GameObject slot;
    void Start()
    {
        UIManager.Instance.RightHand = transform;
        slot = Resources.Load<GameObject>("Canvas/MagicSlot");
    }

    public void AddRightSlot()
    {
        GameObject spawnedObj = Instantiate(slot);
        spawnedObj.transform.parent = transform;
        spawnedObj.transform.SetSiblingIndex(transform.childCount - 2);
    }
}
