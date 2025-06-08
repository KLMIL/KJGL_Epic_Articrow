using System;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class Inventory
    {
        #region [OnLeftHandEvent]
        event Action<List<GameObject>> _onLeftHandEvent;
        public void InitLeftHand()
        {
            _onLeftHandEvent = null;
        }
        public void SingleSubLeftHand(Action<List<GameObject>> newSub)
        {
            _onLeftHandEvent = null;
            _onLeftHandEvent += newSub;
        }
        public void InvokeLeftHand()
        {
            _onLeftHandEvent?.Invoke(_leftList);
        }
        #endregion

        #region [OnRightHandEvent]
        event Action<List<GameObject>> _onRightHandEvent;
        public void InitRightHand()
        {
            _onRightHandEvent = null;
        }
        public void SingleSubRightHand(Action<List<GameObject>> newSub)
        {
            _onRightHandEvent = null;
            _onRightHandEvent += newSub;
        }
        public void InvokeRightHand()
        {
            _onRightHandEvent?.Invoke(_rightList);
        }
        #endregion

        int _maxCount = 8;

        #region [LeftList]
        List<GameObject> _leftList = new List<GameObject>();
        event Action<List<GameObject>> _onUpdateLeftListEvent;
        public void SubUpdateLeftList(Action<List<GameObject>> newSub)
        {
            _onUpdateLeftListEvent += newSub;
        }
        #endregion

        #region [RightList]
        List<GameObject> _rightList = new List<GameObject>();
        event Action<List<GameObject>> _onUpdateRightListEvent;
        public void SubUpdateRightList(Action<List<GameObject>> newSub)
        {
            _onUpdateRightListEvent += newSub;
        }
        #endregion

        #region [InventoryList]
        List<GameObject> _inventoryList = new List<GameObject>();
        event Action<List<GameObject>> _onUpdateInventoryListEvent;
        public void SubUpdateInventoryList(Action<List<GameObject>> newSub)
        {
            _onUpdateInventoryListEvent += newSub;
        }
        #endregion

        public void Init()
        {
            _onLeftHandEvent = null;
            _onRightHandEvent = null;

            _leftList = new List<GameObject>();
            _onUpdateLeftListEvent = null;

            _rightList = new List<GameObject>();
            _onUpdateRightListEvent = null;

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
            //각 슬롯 내용물 확인
            _onUpdateLeftListEvent?.Invoke(_leftList);
            _onUpdateRightListEvent?.Invoke(_rightList);
            _onUpdateInventoryListEvent?.Invoke(_inventoryList);

            //스킬 매니저 초기화 후 슬롯 효과 다시 적용
            GameManager.Instance.LeftSkillManager.CheckSkill(_leftList);
            GameManager.Instance.RightSkillManager.CheckSkill(_rightList);
        }
    }
}