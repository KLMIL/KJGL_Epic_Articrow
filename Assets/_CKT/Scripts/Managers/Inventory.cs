using System;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    [System.Serializable]
    public class Inventory
    {
        #region [OnLeftHandEvent]
        event Action<List<ImageParts>> _onLeftHandEvent;
        public void InitLeftHand()
        {
            _onLeftHandEvent = null;
        }
        public void SingleSubLeftHand(Action<List<ImageParts>> newSub)
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
        event Action<List<ImageParts>> _onRightHandEvent;
        public void InitRightHand()
        {
            _onRightHandEvent = null;
        }
        public void SingleSubRightHand(Action<List<ImageParts>> newSub)
        {
            _onRightHandEvent = null;
            _onRightHandEvent += newSub;
        }
        public void InvokeRightHand()
        {
            _onRightHandEvent?.Invoke(_rightList);
        }
        #endregion

        
        List<ImageParts> _leftList = new List<ImageParts>();
        event Action<List<ImageParts>> _onUpdateLeftListEvent;
        public void SubUpdateLeftList(Action<List<ImageParts>> newSub)
        {
            _onUpdateLeftListEvent += newSub;
        }

        List<ImageParts> _rightList = new List<ImageParts>();
        event Action<List<ImageParts>> _onUpdateRightListEvent;
        public void SubUpdateRightList(Action<List<ImageParts>> newSub)
        {
            _onUpdateRightListEvent += newSub;
        }

        List<ImageParts> _inventoryList = new List<ImageParts>();
        event Action<List<ImageParts>> _onUpdateInventoryListEvent;
        public void SubUpdateInventoryList(Action<List<ImageParts>> newSub)
        {
            _onUpdateInventoryListEvent += newSub;
        }

        int _maxCount = 8;

        public void Init()
        {
            _onLeftHandEvent = null;
            _onRightHandEvent = null;

            _leftList = new List<ImageParts>();
            _onUpdateLeftListEvent = null;

            _rightList = new List<ImageParts>();
            _onUpdateRightListEvent = null;

            _inventoryList = new List<ImageParts>();
            _onUpdateInventoryListEvent = null;
        }

        public bool CheckInventorySlotFull()
        {
            return _inventoryList.Count >= _maxCount;
        }

        /// <summary>
        /// 왼손, 오른손, 인벤토리 내용물 갱신
        /// </summary>
        public void InvokeUpdateList()
        {
            _onUpdateLeftListEvent?.Invoke(_leftList);
            _onUpdateRightListEvent?.Invoke(_rightList);
            _onUpdateInventoryListEvent?.Invoke(_inventoryList);
        }
    }
}