using UnityEngine;

namespace CKT
{
    public class RightSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            GameManager.Instance.Inventory.SubUpdateRightList((list) => base.UpdateItemSlotList(list));
        }
    }
}