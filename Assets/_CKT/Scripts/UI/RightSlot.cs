using UnityEngine;

namespace CKT
{
    public class RightSlot : ItemSlot
    {
        void Start()
        {
            base.Init();

            //BMC.PlayerManager.Instance.Inventory.SkillManager.OnUpdateSlotListActionT1.Register((list) => base.UpdateItemSlotList(list));
        }
    }
}