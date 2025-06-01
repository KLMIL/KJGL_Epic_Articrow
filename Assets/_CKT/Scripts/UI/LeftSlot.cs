using UnityEngine;

namespace CKT
{
    public class LeftSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            GameManager.Instance.Inventory.SubUpdateLeftList((list) => base.UpdateSlot(list));
        }
    }
}