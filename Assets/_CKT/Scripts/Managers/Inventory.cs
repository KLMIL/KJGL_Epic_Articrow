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

        #region [CastEffect]
        event Action<GameObject> _onCastEffectEvent;
        public void InitCastEffect()
        {
            _onCastEffectEvent = null;
        }
        public void SubCastEffect(Action<GameObject> newSub)
        {
            _onCastEffectEvent += newSub;
        }
        public void InvokeCastEffect(GameObject obj)
        {
            _onCastEffectEvent?.Invoke(obj);
        }
        #endregion

        #region[HitEffect]
        event Action<GameObject> _onHitEffectEvent;
        public void InitHitEffect()
        {
            _onHitEffectEvent = null;
        }
        public void SubHitEffect(Action<GameObject> newSub)
        {
            _onHitEffectEvent += newSub;
        }
        public void InvokeHitEffect(GameObject obj)
        {
            _onHitEffectEvent?.Invoke(obj);
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

            _onCastEffectEvent = null;
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