using System;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class Inventory
    {
        int _maxCount = 8;

        #region [InventoryList]
        List<GameObject> _inventoryList = new List<GameObject>();
        event Action<List<GameObject>> _onUpdateInventoryListEvent;
        public void SubUpdateInventoryList(Action<List<GameObject>> newSub)
        {
            _onUpdateInventoryListEvent += newSub;
        }
        void InvokeUpdateInventoryList(List<GameObject> list)
        {
            _onUpdateInventoryListEvent?.Invoke(list);
        }
        #endregion

        public void Init()
        {
            _inventoryList = new List<GameObject>();
            _onUpdateInventoryListEvent = null;
        }

        public bool CheckInventorySlotFull()
        {
            return _inventoryList.Count >= _maxCount;
        }

        /// <summary>
        /// 왼손, 오른손, 인벤토리 내용물 갱신, 왼손, 오른손 스킬 레벨 갱신
        /// </summary>
        public void InvokeUpdateList()
        {
            //인벤토리 내용물 확인
            InvokeUpdateInventoryList(_inventoryList);

            //왼손, 오른손 슬롯 확인 후 효과 다시 적용
            GameManager.Instance.LeftSkillManager.CheckSkill();
            GameManager.Instance.RightSkillManager.CheckSkill();
        }
    }
}