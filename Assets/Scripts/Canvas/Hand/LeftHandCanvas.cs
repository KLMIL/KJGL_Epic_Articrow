using System.Collections.Generic;
using UnityEngine;

public class LeftHandCanvas : MonoBehaviour
{
    GameObject slot;
    void Start()
    {
        UIManager.Instance.LeftHand = transform;
        slot = Resources.Load<GameObject>("Canvas/MagicSlot");
    }

    public void AddLeftSlot()
    {
        GameObject spawnedObj = Instantiate(slot);
        spawnedObj.transform.parent = transform;
        spawnedObj.transform.SetSiblingIndex(transform.childCount - 2);
    }
}
