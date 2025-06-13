using UnityEngine;

namespace CKT
{
    public class LeftSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            GameManager.Instance.LeftSkillManager.OnUpdateSlotListActionT1.Register((list) => base.UpdateItemSlotList(list));
        }
    }
}