using System;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class Inventory
    {
        #region [InventoryList]
        List<GameObject> _inventoryList = new List<GameObject>();
        #endregion

        #region [OnUpdateInventoryListActionT1]
        public ActionT1Handler<List<GameObject>> OnUpdateInventoryListActionT1 = new();
        #endregion

        #region [Slot Count]
        Func<int> _getSlotCountInt;
        public void SingleSubSlotCount(Func<int> newSub)
        {
            _getSlotCountInt = null;
            _getSlotCountInt += newSub;
        }
        int InvokeSlotCount()
        {
            return _getSlotCountInt.Invoke();
        }
        #endregion

        public void Init()
        {
            _inventoryList = new List<GameObject>();
            OnUpdateInventoryListActionT1.Init();
            _getSlotCountInt = null;
        }

        public bool CheckInventorySlotFull()
        {
            return _inventoryList.Count >= InvokeSlotCount();
        }

        /// <summary>
        /// 왼손, 오른손, 인벤토리 내용물 갱신, 왼손, 오른손 스킬 레벨 갱신
        /// </summary>
        public void InvokeUpdateList()
        {
            //인벤토리 내용물 확인
            OnUpdateInventoryListActionT1.Trigger(_inventoryList);

            //왼손, 오른손 슬롯 확인 후 효과 다시 적용
            GameManager.Instance.LeftSkillManager.CheckSkill();
            GameManager.Instance.RightSkillManager.CheckSkill();
        }
    }
}