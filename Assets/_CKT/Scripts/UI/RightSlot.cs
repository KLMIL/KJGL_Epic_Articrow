using UnityEngine;

namespace CKT
{
    public class RightSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            GameManager.Instance.RightSkillManager.SubUpdateList((list) => base.UpdateItemSlotList(list));
        }
    }
}