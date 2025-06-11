using UnityEngine;

namespace CKT
{
    public class LeftSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            GameManager.Instance.LeftSkillManager.SubUpdateList((list) => base.UpdateItemSlotList(list));
        }
    }
}