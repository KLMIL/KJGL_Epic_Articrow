using UnityEngine;

namespace CKT
{
    public class RightSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            GameManager.Instance.RightSkillManager.OnUpdateListActionT1.Register((list) => base.UpdateItemSlotList(list));
        }
    }
}